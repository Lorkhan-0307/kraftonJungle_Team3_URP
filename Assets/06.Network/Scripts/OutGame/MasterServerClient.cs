using Photon.Pun;
using System.Collections;
using UnityEngine;
using TMPro;
using Amazon.Runtime.Internal.Auth;

public class MasterServerClient : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public OutgameRoomsManager orManager;
    [HideInInspector]
    public MyRoomManager mrManager;

    [SerializeField]
    TMP_InputField nicknameInput;

    DynamoDBManager dbManager;

    void Start()
    {
        orManager = FindObjectOfType<OutgameRoomsManager>(true);
        mrManager = FindObjectOfType<MyRoomManager>(true);

        // Photon 서버에 연결
        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("ConnectUsingSettings");
        }
        else if(PhotonNetwork.InRoom)
        {
            Debug.Log(PhotonNetwork.NetworkClientState.ToString());
            PhotonNetwork.LeaveRoom();
            Debug.Log("LeaveRoom");
        }

        // 일정 주기로 실행되는 새로고침 코루틴 실행
        StartCoroutine(RefreshPeriod());

        LoginWithToken();
    }

    public async void LoginWithToken()
    {
        // DB 통해 로그인
        dbManager=GetComponent<DynamoDBManager>();

        string token = LoginTokenManager.LoadDataWithToken();

        PlayerData playerData = new PlayerData();

        await dbManager.LoadData(token, playerData);

        Debug.Log($"Loaded {playerData.Nickname}");
        nicknameInput.text = playerData.Nickname;
        PhotonNetwork.LocalPlayer.NickName = playerData.Nickname;
        LoginTokenManager.SaveTokenToLocal(playerData.UserToken);
    }
    public async void UpdateNickName()
    {
        string token = LoginTokenManager.LoadDataWithToken();
        string name = nicknameInput.text;

        await dbManager.UpdateNickname(token, name);
        PhotonNetwork.LocalPlayer.NickName = name;
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

    public override void OnLeftRoom()
    {
        PhotonNetwork.JoinLobby();

        ExitGames.Client.Photon.Hashtable customData = new ExitGames.Client.Photon.Hashtable();
        customData.Add("IsReady", false);
        PhotonNetwork.LocalPlayer.CustomProperties = customData;
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