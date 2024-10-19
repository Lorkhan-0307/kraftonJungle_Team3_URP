using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkManager : MonoBehaviourPunCallbacks
{
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
        // �濡 �����ϸ� �÷��̾� ĳ���͸� ����
        PhotonNetwork.Instantiate("PlayerPrefab", Vector3.zero, Quaternion.identity);

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
        string result = "";
        result += from.ViewID.ToString();
        result += ",";
        result += to.ViewID.ToString();

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
}
