using Photon.Pun;
using System.Collections;
using UnityEngine;
using TMPro;
using Michsky.UI.Dark;

public class MasterServerClient : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public OutgameRoomsManager orManager;
    [HideInInspector]
    public MyRoomManager mrManager;

    [SerializeField]
    TMP_InputField nicknameInput;

    DynamoDBManager dbManager;

    bool isPhotonConnected = false;
    bool isDBConnected = false;

    void Start()
    {
        PhotonNetwork.SerializationRate = 20; // 초당 20회 동기화

        orManager = FindObjectOfType<OutgameRoomsManager>(true);
        mrManager = FindObjectOfType<MyRoomManager>(true);
        dbManager = GetComponent<DynamoDBManager>();

        // Photon 서버에 연결
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            LoginWithToken();
        }

        // 일정 주기로 실행되는 새로고침 코루틴 실행
        StartCoroutine(RefreshPeriod());
    }

    public async void LoginWithToken()
    {
        // DB 통해 로그인
        string token = LoginTokenManager.GetToken();

        PlayerData playerData = new PlayerData();

        await dbManager.LoadData(token, playerData);

        nicknameInput.text = playerData.Nickname;
        PhotonNetwork.LocalPlayer.NickName = playerData.Nickname;
        LoginTokenManager.SaveTokenToLocal(playerData.UserToken);

        OnJoinedDB();
    }
    public async void UpdateNickName()
    {
        string token = LoginTokenManager.GetToken();
        string name = nicknameInput.text;

        name = name.Trim();
        if (name == "")
            return;

        nicknameInput.text = name;

        await dbManager.UpdateNickname(token, name);
        PhotonNetwork.LocalPlayer.NickName = name;
    }

    public async void ResetButton()
    {
        // DB에 있는 데이터 삭제
        await dbManager.DeletePlayerDataByToken(LoginTokenManager.GetToken());

        LoginTokenManager.ResetToken();

        LoginWithToken();
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
    public override void OnJoinedLobby()
    {
        isPhotonConnected = true;
        OnConnectFinished();
    }
    void OnJoinedDB()
    {
        isDBConnected = true;
        OnConnectFinished();
    }

    void OnConnectFinished()
    {
        if(isPhotonConnected && isDBConnected)
        {
            //TODO: 로딩 완료
            Debug.Log("로딩 완료!");
            FindObjectOfType<SplashScreenManager>().skipOnAnyKeyPress = true;
        }
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