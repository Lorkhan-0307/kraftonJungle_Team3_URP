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
    SwitchDayNight,
    HungerGauge,
    AccelTime,
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
        Debug.Log("StartGame Button Clicked");
        if (NetworkManager.Instance.curState != GameState.OnRoom) return;

        //if(PhotonNetwork.PlayerList.Length < requirePlayers)
        //{
        //    Debug.Log($"[{PhotonNetwork.PlayerList.Length}/{requirePlayers}] 인원이 부족합니다.");
        //    return;
        //}

        // TODO: 게임 시작 기능 구현
        SetPlayerRole();
    }



    #region GameLogic
    public int monsterActorNum = -1;

    public bool[] isAlivePlayers;


    public void PlayerDeath(int actorNum)
    {
        isAlivePlayers[actorNum-1] = false;

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
        for(int i = 0; i < isAlivePlayers.Length; i++ )
        {
            if(isAlivePlayers[i])
                result.Add(i+1);
        }

        return result;
    }

    public void CheckEndCondition()
    {
        bool isMonsterAlive = isAlivePlayers[monsterActorNum-1];

        // 괴물이 죽었을 경우
        if (!isMonsterAlive)
        {
            EndGame();
            return;
        }

        // 괴물이 살아있을 경우 모든 플레이어가 죽어야 게임 종료
        for(int i = 0; i < isAlivePlayers.Length; i++)
        {
            if (i == monsterActorNum-1) continue;

            if (isAlivePlayers[i])
                return;
        }

        EndGame();
    }

    #endregion


    #region GameStart
    public void SetPlayerRole()
    {
        // 현재 룸에 접속되어있는 플레이어 목록을 가져옴
        Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;

        // 플레이어 생존 여부 배열 초기화
        isAlivePlayers = new bool[playerList.Length];

        for (int i = 0; i < isAlivePlayers.Length; i++)
        {
            isAlivePlayers[i] = true;
        }


        // 랜덤으로 몬스터 번호 할당
        Random.InitState((int)Time.time);
        monsterActorNum = Random.Range(0, playerList.Length)+1;

        Vector3[] randomSpawnPos = new Vector3[playerList.Length];
        // 각 플레이어에게 랜덤 스폰 위치와 몬스터 번호를 전송
        for (int i = 0; i < randomSpawnPos.Length; i++)
        {
            // 각 플레이어의 랜덤 스폰 위치 설정
            randomSpawnPos[i] = NPCManager.GetRandomNavMeshPosition();

            Debug.Log($"{i}: {randomSpawnPos[i]}");
        }
        // 이벤트 데이터에 스폰 위치와 몬스터 번호를 담음
        object[] eventData = new object[] { randomSpawnPos, monsterActorNum };

        // 이벤트 전송 (몬스터 번호와 랜덤 스폰 위치)
        NetworkManager.Instance.SendToClients(EventCode.GameStart, eventData);

        // 필드에 랜덤으로 NPC 뿌림
        FindObjectOfType<NPCManager>().SpawnNPC();
    }
    #endregion

    #region Time

    /// <summary>
    /// 서버가 밤낮 바뀐 시점에 실행합니다.
    /// </summary>
    /// <param name="isDay"></param>
    public void SwitchDayNight(bool isDay)
    {
        NetworkManager.Instance.SendToClients(EventCode.SwitchDayNight, isDay);
    }
    #endregion
}
