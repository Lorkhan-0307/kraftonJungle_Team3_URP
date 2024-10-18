using Photon.Pun;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameState curState;

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

    void Start()
    {
        // Photon ������ ����
        PhotonNetwork.ConnectUsingSettings();
    }

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
}
