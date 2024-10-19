using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine.InputSystem;
using Unity.VisualScripting; // 이벤트 코드에 사용

public enum EventCode
{
    GameStart = 0,
    AttackRequest,
    PlayerDeath,
    EndGame,
    SwitchDayNight
}

public enum GameState
{
    OnRoom,
    Playing,
    End,
}

public class ServerLogic : MonoBehaviourPunCallbacks
{
    public int requirePlayers = 5;

    public GameState curState = GameState.OnRoom;

    private InputAction gameStartAction;
    // 싱글톤 인스턴스
    public static ServerLogic Instance { get; private set; }

    // Awake는 Start보다 먼저 호출됩니다.
    private void Awake()
    {
        // 싱글톤 인스턴스가 없는 경우 이 객체를 인스턴스로 설정
        if (Instance == null)
        {
            Instance = this;
            // 다른 씬에서도 파괴되지 않도록 설정
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 인스턴스가 존재하는 경우, 중복된 객체를 파괴
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameStartAction = new InputAction(type: InputActionType.Value, binding: "<Keyboard>/space");
        gameStartAction.Enable();
        gameStartAction.performed += StartGame;
    }

    public void StartGame(InputAction.CallbackContext context)
    {
        if (curState != GameState.OnRoom) return;
        
        if(PhotonNetwork.PlayerList.Length < requirePlayers)
        {
            Debug.Log($"[{PhotonNetwork.PlayerList.Length}/{requirePlayers}] 인원이 부족합니다.");
            return;
        }

        // TODO: 게임 시작 기능 구현
        curState = GameState.Playing;

    }


    // Update is called once per frame
    void Update()
    {
        ApplyTime();
    }

    #region GameLogic
    public int monsterNum = -1;

    public bool[] isAlivePlayers;


    public void PlayerDeath(int actorNum)
    {
        isAlivePlayers[actorNum] = false;

        CheckEndCondition();
    }
    /// <summary>
    /// 게임 종료조건이 달성되었을 때 실행시켜주세요.
    /// Run when the game end condition is in effect.
    /// </summary>
    public void EndGame()
    {
        string result = "";

        List<int> players = GetAlivePlayers();

        result += players[0].ToString();

        for (int i = 1; i < players.Count; i++)
        {
            result += ",";
            result += players[i].ToString();
        }

        NetworkManager.Instance.SendToClients(EventCode.EndGame, result);
    }

    public List<int> GetAlivePlayers()
    {
        List<int> result = new List<int>();
        for(int i = 1; i < isAlivePlayers.Length; i++ )
        {
            if(isAlivePlayers[i])
                result.Add(i);
        }

        return result;
    }

    public void CheckEndCondition()
    {
        bool isMonsterAlive = isAlivePlayers[monsterNum];

        // 괴물이 죽었을 경우
        if (!isMonsterAlive)
        {
            EndGame();
            return;
        }

        // 괴물이 살아있을 경우 모든 플레이어가 죽어야 게임 종료
        for(int i = 1; i < isAlivePlayers.Length; i++)
        {
            if (i == monsterNum) continue;

            if (isAlivePlayers[i])
                return;
        }

        EndGame();
    }

    #endregion


    #region GameStart
    public void SetPlayerRole()
    {
        // 현재 룸에 접속되어있는 플레이어 목록 받아옴
        Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;
        isAlivePlayers = new bool[playerList.Length+1];
        for (int i = 1; i < isAlivePlayers.Length; i++)
        {
            isAlivePlayers[i] = true;
        }

        Debug.Log("플레이어 수: " + playerList.Length.ToString());
        monsterNum = Random.Range(0, playerList.Length)+1;

        NetworkManager.Instance.SendToClients(EventCode.GameStart, monsterNum);
    }
    #endregion

    #region Time
    public float dayLength;
    public float nightLength;
    public float curTime;
    public bool isDay = true;

    public void ApplyTime()
    {
        curTime += Time.deltaTime;

        if (isDay)
        {
            if (curTime > dayLength)
            {
                // 저녁됨 방송함
            }
        }
        else
        {
            if (curTime > nightLength)
            {
                // 아침됨 방송함
            }
        }

        // 현재시각 방송함
    }

    #endregion
}
