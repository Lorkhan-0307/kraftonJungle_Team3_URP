using Photon.Pun;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameState curState;

    // 싱글톤 인스턴스
    public static NetworkManager Instance { get; private set; }

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

    void Start()
    {
        // Photon 서버에 연결
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // 서버에 연결되면 방에 입장 시도
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 방이 없으면 새로운 방을 생성
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        // 방에 입장하면 플레이어 캐릭터를 생성
        PhotonNetwork.Instantiate("PlayerPrefab", Vector3.zero, Quaternion.identity);

        // 방장(마스터 클라이언트)일 경우 특정 로직 실행
        if (PhotonNetwork.IsMasterClient)
        {
            gameObject.AddComponent<ServerLogic>();
        }
    }
}
