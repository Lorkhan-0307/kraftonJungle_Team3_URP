using Michsky.UI.Dark;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class MasterServerClient : MonoBehaviourPunCallbacks
{
    public OutgameRoomsManager orManager;
    public MyRoomManager mrManager;

    void Start()
    {
        orManager = FindObjectOfType<OutgameRoomsManager>(true);
        mrManager = FindObjectOfType<MyRoomManager>(true);

        // Photon 서버에 연결
        if(!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
        else if(!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();

        // 일정 주기로 실행되는 새로고침 코루틴 실행
        StartCoroutine(RefreshPeriod());
    }

    IEnumerator RefreshPeriod()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (PhotonNetwork.IsConnected)
            {
                orManager.UpdateRoomList();
            }

            if (PhotonNetwork.InRoom)
            {
                mrManager.UpdatePlayerList();

                mrManager.UpdatePing();
                // 임시. 매 초 플레이어 목록 새로고침
                mrManager.CallUpdatePlayerList();
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();

        ExitGames.Client.Photon.Hashtable customData = new ExitGames.Client.Photon.Hashtable();
        customData.Add("IsReady", false);
        PhotonNetwork.LocalPlayer.CustomProperties = customData;
    }

    [PunRPC]
    public void SendChat()
    {

    }
}

public class ChatData
{
    public string senderID;
    public string text;

    public ChatData() { }
    public ChatData(string senderID, string text)
    {
        this.senderID = senderID;
        this.text = text;
    }

    public static object[] InstanceToData(ChatData data)
    {
        return new object[] { data.senderID, data.text };
    }
    public static ChatData DataToInstance(object data)
    {
        object[] datas = (object[])data;
        return new ChatData((string)datas[0], (string)datas[1]);
    }
}