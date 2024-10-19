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
    public TimeSwitchSlider timeswitchslider;
    #region Singleton
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

    #endregion

    #region RoomServer
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
        // 방장(마스터 클라이언트)일 경우 특정 로직 실행
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
    /// 죽은 플레이어 본인이 죽은 시점에 실행시켜주세요.
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
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }; // 모든 클라이언트에게 전송
        SendOptions sendOptions = new SendOptions { Reliability = true }; // 신뢰성 보장

        //이거 쓰면 될듯
        PhotonNetwork.RaiseEvent((byte)code, content, raiseEventOptions, sendOptions);
    }
    public void SendToClients(EventCode code, object content)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // 모든 클라이언트에게 전송
        SendOptions sendOptions = new SendOptions { Reliability = true }; // 신뢰성 보장

        //이거 쓰면 될듯
        PhotonNetwork.RaiseEvent((byte)code, content, raiseEventOptions, sendOptions);
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

        //TODO: 자신의 플레이어 ActorNumber 가 전송받은 id와 같은지 비교하고 몬스터, 연구원으로 초기화함.
        Debug.Log($"Monster : {monsterNum}");
        Vector3 myPosition = spawnPos[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        Debug.Log(myPosition.ToString());
        GameObject spawnedPlayer = null;
        if (monsterNum == PhotonNetwork.LocalPlayer.ActorNumber)
            spawnedPlayer = PhotonNetwork.Instantiate(playerMonsterName, myPosition, Quaternion.identity);
        else
            spawnedPlayer = PhotonNetwork.Instantiate(playerScientistName, myPosition, Quaternion.identity);

        myPlayer = spawnedPlayer.GetComponent<Player>();

        GameObject po = Instantiate(playerObjectPrefab, spawnedPlayer.transform.position, spawnedPlayer.transform.rotation, spawnedPlayer.transform);

        curState = GameState.Playing;
        GameManager.instance.StartGame();
    }
    #endregion

    void Start()
    {
        testAction.Enable();
        testAction.performed += Test;
        // Photon 서버에 연결
        PhotonNetwork.ConnectUsingSettings();
    }



    public InputAction testAction;
    public void Test(InputAction.CallbackContext context)
    {
        GetComponent<ServerLogic>().SetPlayerRole();
    }

    // 모든 클라이언트에게 이벤트를 보냄.
    public void HungerEvent(bool ishungerzero)
    {
        SendToClients(EventCode.HungerGauge, ishungerzero);
    }

    public void TimeAccel(float SkipTime)
    {
        SendToClients(EventCode.AccelTime, SkipTime);
    }

    public bool IsMonster()
    {
        return (myPlayer.type == CharacterType.Monster);
    }
}
