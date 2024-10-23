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
        PhotonNetwork.ConnectUsingSettings();
        StartCoroutine(RefreshPeriod());
    }

    IEnumerator RefreshPeriod()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("RefreshRoomsPeriod");
                orManager.UpdateRoomList();
            }

            if (PhotonNetwork.InRoom)
            {
                mrManager.UpdatePlayerList();
                Debug.Log("RenewPing");

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
}
