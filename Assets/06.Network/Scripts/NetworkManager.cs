using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem;

public class NetworkManager : MonoBehaviourPunCallbacks
{
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
        testAction.Enable();
        testAction.performed += Test;
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
        //PhotonNetwork.Instantiate("PlayerPrefab", Vector3.zero, Quaternion.identity);

        // 방장(마스터 클라이언트)일 경우 특정 로직 실행
        if (PhotonNetwork.IsMasterClient)
        {
            gameObject.AddComponent<ServerLogic>();
        }
    }


    public InputAction testAction;
    public void Test(InputAction.CallbackContext context)
    {
        GetComponent<ServerLogic>().SetPlayerRole();
    }

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
    /// 죽은 플레이어 본인이 실행시켜주세요.
    /// </summary>
    public void PlayerDeath()
    {
        int id = PhotonNetwork.LocalPlayer.ActorNumber;

        SendToClients(EventCode.PlayerDeath, id);
    }

    /// <summary>
    /// 게임 종료조건이 달성되었을 때 실행시켜주세요.
    /// </summary>
    public void PlayerWin()
    {
        // TODO: 살아있는 플레이어 목록을 문자열로 만들어서 보내주자.
        // SendToClients(EventCode.PlayerDeath, actorNumber);
    }

    /// <summary>
    /// 0: Day, 1:Night
    /// </summary>
    public void SwitchDayNight()
    {
        //SendToClients(EventCode.SwitchDayNight);
    }
    #endregion

    public void SendToServer(EventCode code, object content)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }; // 모든 클라이언트에게 전송
        SendOptions sendOptions = new SendOptions { Reliability = true }; // 신뢰성 보장

        //이거 쓰면 될듯
        PhotonNetwork.RaiseEvent(0, content, raiseEventOptions, sendOptions);
    }
    public void SendToClients(EventCode code, object content)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // 모든 클라이언트에게 전송
        SendOptions sendOptions = new SendOptions { Reliability = true }; // 신뢰성 보장

        //이거 쓰면 될듯
        PhotonNetwork.RaiseEvent(0, content, raiseEventOptions, sendOptions);
    }
}
