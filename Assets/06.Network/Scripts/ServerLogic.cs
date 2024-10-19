using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine.InputSystem;
using Unity.VisualScripting; // �̺�Ʈ �ڵ忡 ���

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
    // �̱��� �ν��Ͻ�
    public static ServerLogic Instance { get; private set; }

    // Awake�� Start���� ���� ȣ��˴ϴ�.
    private void Awake()
    {
        // �̱��� �ν��Ͻ��� ���� ��� �� ��ü�� �ν��Ͻ��� ����
        if (Instance == null)
        {
            Instance = this;
            // �ٸ� �������� �ı����� �ʵ��� ����
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // �̹� �ν��Ͻ��� �����ϴ� ���, �ߺ��� ��ü�� �ı�
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
            Debug.Log($"[{PhotonNetwork.PlayerList.Length}/{requirePlayers}] �ο��� �����մϴ�.");
            return;
        }

        // TODO: ���� ���� ��� ����
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
    /// ���� ���������� �޼��Ǿ��� �� ��������ּ���.
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

        // ������ �׾��� ���
        if (!isMonsterAlive)
        {
            EndGame();
            return;
        }

        // ������ ������� ��� ��� �÷��̾ �׾�� ���� ����
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
        // ���� �뿡 ���ӵǾ��ִ� �÷��̾� ��� �޾ƿ�
        Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;
        isAlivePlayers = new bool[playerList.Length+1];
        for (int i = 1; i < isAlivePlayers.Length; i++)
        {
            isAlivePlayers[i] = true;
        }

        Debug.Log("�÷��̾� ��: " + playerList.Length.ToString());
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
                // ����� �����
            }
        }
        else
        {
            if (curTime > nightLength)
            {
                // ��ħ�� �����
            }
        }

        // ����ð� �����
    }

    #endregion
}
