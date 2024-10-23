using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameSettings gameSettings;
    public GameState curState = GameState.OnRoom;

    public Player myPlayer;

    public int NPCCount = 0;

    public bool IsServer()
    {
        return PhotonNetwork.IsMasterClient;
    }


    public Monster Monster { get { return FindObjectOfType<Monster>(); } }
    public bool IsMonster() {  return (myPlayer.type == CharacterType.Monster); }

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
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected Cause: {cause.ToString()}");
    }
    #endregion

    #region Static Methods:Sender
    public static void SendToServer(EventCode code, object content)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }; // 모든 클라이언트에게 전송
        SendOptions sendOptions = new SendOptions { Reliability = true }; // 신뢰성 보장

        //이거 쓰면 될듯
        PhotonNetwork.RaiseEvent((byte)code, content, raiseEventOptions, sendOptions);
    }
    public static void SendToClients(EventCode code, object content)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // 모든 클라이언트에게 전송
        SendOptions sendOptions = new SendOptions { Reliability = true }; // 신뢰성 보장

        //이거 쓰면 될듯
        PhotonNetwork.RaiseEvent((byte)code, content, raiseEventOptions, sendOptions);
    }
    #endregion

}
