using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    public Dictionary<int, Monster> Monsters
    {
        get
        {
            if (monsters == null || monsters.Keys.Count == 0)
            {
                monsters = new Dictionary<int, Monster>();
                Monster[] ms = FindObjectsOfType<Monster>();
                foreach(Monster m in ms)
                {
                    monsters.Add(m.GetComponent<PhotonView>().OwnerActorNr, m);
                }
            }
            return monsters;
        }
    }
    Dictionary<int, Monster> monsters = null;
    public bool IsMonster() {
        if (myPlayer == null) return false;
        return (myPlayer.type == CharacterType.Monster);
    }

    public void LoadGameSettings()
    {
        gameSettings = new GameSettings(); //Resources.Load<GameSettings>("GameSettingsData");
    }

    #region Singleton
    // 싱글톤 인스턴스
    public static NetworkManager Instance { get; set; }

    // Awake는 Start보다 먼저 호출됩니다.
    private void Awake()
    {
        // 싱글톤 인스턴스가 이미 있는 경우 삭제하고 새 오브젝트 생성
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        // 다른 씬에서도 파괴되지 않도록 설정
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region RoomServer
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected Cause: {cause.ToString()}");
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
        Instance = null;
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
