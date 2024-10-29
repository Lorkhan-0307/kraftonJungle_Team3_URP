using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ServerButtonElements
{
    public string serverName;
    public string serverOwner;
    public string serverPing;
    public string serverPlayerNum;
    public string serverTotalPlayerNum;

    public ServerButtonElements(RoomInfo room)
    {
        serverName = room.Name;
        serverOwner = room.CustomProperties["Master"]?.ToString();
        serverPing = room.CustomProperties["Ping"]?.ToString();    // TODO : 핑 데이터 받아오기
        serverPlayerNum = room.PlayerCount.ToString();
        serverTotalPlayerNum = room.MaxPlayers.ToString();
    }
}

public class ServerButton : MonoBehaviour
{
    [SerializeField] private TMP_Text serverName;
    [SerializeField] private TMP_Text serverOwner;
    [SerializeField] private TMP_Text serverPing;
    [SerializeField] private TMP_Text serverPlayerNum;

    
    
    public void SetupServerButton(ServerButtonElements sbe)
    {
        serverName.text = sbe.serverName;
        serverOwner.text = sbe.serverOwner;
        serverPlayerNum.text = sbe.serverPlayerNum + " / " + sbe.serverTotalPlayerNum;
        serverPing.text = sbe.serverPing;


    }

    public void OnClickServerButton()
    {
        // Todo :여기에서 누른 서버에 접속하는 기능을 넣어주시면 됩니다.
        // 테스트를 위해 Debug를 달아두겠습니다.
        
        Debug.Log("Clicked " + serverName.text + "Server Button");
        PhotonNetwork.JoinRoom(serverName.text);


        // 참여하게 되는 경우이므로, Guest로 참여합니다.
        FindObjectOfType<MyRoomManager>().OnRoomCreateOrJoin(false);
    }
}
