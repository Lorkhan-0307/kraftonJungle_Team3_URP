using Michsky.UI.Dark;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class MasterServerClient : MonoBehaviourPunCallbacks
{
    //public OutgameRoomsManager orManager;

    void Start()
    {
        // Photon 서버에 연결
        PhotonNetwork.ConnectUsingSettings();

        StartCoroutine(RenewPing());
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();

        ExitGames.Client.Photon.Hashtable customData = new ExitGames.Client.Photon.Hashtable();
        customData.Add("IsReady", false);
        PhotonNetwork.LocalPlayer.CustomProperties = customData;
    }


    IEnumerator RenewPing()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            if(PhotonNetwork.InRoom)
            {
                ExitGames.Client.Photon.Hashtable data = PhotonNetwork.CurrentRoom.CustomProperties;
                if(data.ContainsKey("Ping"))
                {
                    int ping = PhotonNetwork.GetPing();
                    data["Ping"] = ping;
                    //Debug.Log($"Ping Renewed: {ping}");

                    PhotonNetwork.CurrentRoom.SetCustomProperties(data);
                }
            }
        }
    }
}
