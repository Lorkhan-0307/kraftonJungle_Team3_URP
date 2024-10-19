using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameState curState = GameState.OnRoom;

    Player myPlayer;



    public Monster monster;
    #region Singleton
    // �̱��� �ν��Ͻ�
    public static NetworkManager Instance { get; private set; }

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

    #endregion

    #region RoomServer
    public override void OnConnectedToMaster()
    {
        // ������ ����Ǹ� �濡 ���� �õ�
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ���� ������ ���ο� ���� ����
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        // ����(������ Ŭ���̾�Ʈ)�� ��� Ư�� ���� ����
        if (PhotonNetwork.IsMasterClient)
        {
            gameObject.AddComponent<ServerLogic>();
        }
    }
    #endregion

    #region Methods:Request
    public void AttackEntity(PhotonView from, PhotonView to)
    {
        object[] result = { from.ViewID, to.ViewID };

        SendToClients(EventCode.AttackRequest, result);
    }

    /// <summary>
    /// ���� �÷��̾� ������ ���� ������ ��������ּ���.
    /// Run by the dead player.
    /// </summary>
    public void PlayerDeath()
    {
        int id = PhotonNetwork.LocalPlayer.ActorNumber;

        SendToClients(EventCode.PlayerDeath, id);
    }

    /// <summary>
    /// 0: Day, 1:Night
    /// </summary>
    public void SwitchDayNight(bool isDay)
    {
        SendToClients(EventCode.SwitchDayNight, isDay);
    }
    public void SendToServer(EventCode code, object content)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }; // ��� Ŭ���̾�Ʈ���� ����
        SendOptions sendOptions = new SendOptions { Reliability = true }; // �ŷڼ� ����

        //�̰� ���� �ɵ�
        PhotonNetwork.RaiseEvent(0, content, raiseEventOptions, sendOptions);
    }
    public void SendToClients(EventCode code, object content)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // ��� Ŭ���̾�Ʈ���� ����
        SendOptions sendOptions = new SendOptions { Reliability = true }; // �ŷڼ� ����

        //�̰� ���� �ɵ�
        PhotonNetwork.RaiseEvent(0, content, raiseEventOptions, sendOptions);
    }
    #endregion

    #region StartGame
    [SerializeField]
    string playerScientistName;
    [SerializeField]
    string playerMonsterName;
    [SerializeField]
    GameObject playerObjectPrefab;
    public void StartGame(object data)
    {
        object[] datas = (object[])data;

        Vector3[] spawnPos = (Vector3[])datas[0];
        int monsterNum = (int)datas[1];

        //TODO: �ڽ��� �÷��̾� ActorNumber �� ���۹��� id�� ������ ���ϰ� ����, ���������� �ʱ�ȭ��.
        Debug.Log($"Monster : {monsterNum}");
        Vector3 myPosition = spawnPos[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        Debug.Log(myPosition.ToString());
        GameObject go = null;
        if (monsterNum == PhotonNetwork.LocalPlayer.ActorNumber)
            go = PhotonNetwork.Instantiate(playerMonsterName, myPosition, Quaternion.identity);
        else
            go = PhotonNetwork.Instantiate(playerScientistName, myPosition, Quaternion.identity);

        myPlayer = go.GetComponent<Player>();

        GameObject po = Instantiate(playerObjectPrefab, go.transform.position, go.transform.rotation, go.transform);

        curState = GameState.Playing;
    }
    #endregion

    void Start()
    {
        testAction.Enable();
        testAction.performed += Test;
        // Photon ������ ����
        PhotonNetwork.ConnectUsingSettings();
    }



    public InputAction testAction;
    public void Test(InputAction.CallbackContext context)
    {
        GetComponent<ServerLogic>().SetPlayerRole();
    }

    // ��� Ŭ���̾�Ʈ���� �̺�Ʈ�� ����.
    public void HungerEvent(bool ishungerzero)
    {
        SendToClients(EventCode.HungerGauge, ishungerzero);
    }

    public bool IsMonster()
    {
        return (myPlayer.type == CharacterType.Monster);
    }
}
