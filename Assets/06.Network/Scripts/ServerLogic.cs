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
    SwitchDayNight,
    HungerGauge,
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
        Debug.Log("StartGame Button Clicked");
        if (NetworkManager.Instance.curState != GameState.OnRoom) return;

        //if(PhotonNetwork.PlayerList.Length < requirePlayers)
        //{
        //    Debug.Log($"[{PhotonNetwork.PlayerList.Length}/{requirePlayers}] �ο��� �����մϴ�.");
        //    return;
        //}

        // TODO: ���� ���� ��� ����
        SetPlayerRole();
    }


    // Update is called once per frame
    void Update()
    {
        ApplyTime();
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
        for(int i = 0; i < isAlivePlayers.Length; i++ )
        {
            if(isAlivePlayers[i])
                result.Add(i);
        }

        return result;
    }

    public void CheckEndCondition()
    {
        bool isMonsterAlive = isAlivePlayers[monsterActorNum-1];

        // ������ �׾��� ���
        if (!isMonsterAlive)
        {
            EndGame();
            return;
        }

        // ������ ������� ��� ��� �÷��̾ �׾�� ���� ����
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
        // ���� �뿡 ���ӵǾ��ִ� �÷��̾� ����� ������
        Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;

        // �÷��̾� ���� ���� �迭 �ʱ�ȭ
        isAlivePlayers = new bool[playerList.Length];

        for (int i = 0; i < isAlivePlayers.Length; i++)
        {
            isAlivePlayers[i] = true;
        }


        // �������� ���� ��ȣ �Ҵ�
        int monsterActorNum = Random.Range(0, playerList.Length)+1;

        Vector3[] randomSpawnPos = new Vector3[playerList.Length];
        // �� �÷��̾�� ���� ���� ��ġ�� ���� ��ȣ�� ����
        for (int i = 0; i < randomSpawnPos.Length; i++)
        {
            // �� �÷��̾��� ���� ���� ��ġ ����
            randomSpawnPos[i] = NPCManager.GetRandomNavMeshPosition();

            Debug.Log($"{i}: {randomSpawnPos[i]}");
        }
        // �̺�Ʈ �����Ϳ� ���� ��ġ�� ���� ��ȣ�� ����
        object[] eventData = new object[] { randomSpawnPos, monsterActorNum };

        // �̺�Ʈ ���� (���� ��ȣ�� ���� ���� ��ġ)
        NetworkManager.Instance.SendToClients(EventCode.GameStart, eventData);

        // �ʵ忡 �������� NPC �Ѹ�
        FindObjectOfType<NPCManager>().SpawnNPC();
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
