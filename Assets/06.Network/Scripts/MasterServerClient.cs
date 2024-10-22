using Michsky.UI.Dark;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class MasterServerClient : MonoBehaviourPunCallbacks
{
    //public OutgameRoomsManager orManager;
    [SerializeField]
    MainPanelManager mpManager;

    void Start()
    {
        // Photon 서버에 연결
        PhotonNetwork.ConnectUsingSettings();

        StartCoroutine(RenewPing());
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"[{PhotonNetwork.CurrentRoom.Name}] 방에 입장하였습니다.");

        mpManager.OpenPanel("MyRoom");
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
                    Debug.Log($"Ping Renewed: {ping}");
                }
                
                PhotonNetwork.CurrentRoom.SetCustomProperties(data);
            }
        }
    }
}
