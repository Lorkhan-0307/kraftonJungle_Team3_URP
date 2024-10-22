using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Michsky.UI.Dark;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;



public class CreateRoom : MonoBehaviour
{
    [SerializeField] private TMP_Text serverName;
    [SerializeField] private SliderManager totalPlayerNumSlider;
    [SerializeField] private SwitchManager connectionTypeSwitch; 
    
    

    public void OnClickCreateRoom()
    {
        // 현재 값들을 가지고 사용하면 됩니다.
        string _serverName = serverName.text;
        // 최대 접속 가능 유저수는 slider value입니다.
        int _maxPlayerNum = (int)totalPlayerNumSlider.saveValue;
        // 헷갈릴만한건 connectionType. True면 public입니다.
        bool _connectionType = connectionTypeSwitch.isOn;
        
        
        
        // Todo : 이 값들을 이용해서 서버에서 방을 만드는 코드를 여기에 작성하시면, Create Room Button을 누르면 동작합니다.
        // 현재는 동시에 My Room 으로 넘어가도록 되어있습니다. Loading은 추후에 추가하겠습니다.
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = _maxPlayerNum;

        ExitGames.Client.Photon.Hashtable customData = new ExitGames.Client.Photon.Hashtable();
        customData.Add("IsPublic", _connectionType);
        customData.Add("Ping", PhotonNetwork.GetPing());

        roomOptions.CustomRoomProperties = customData;
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "IsPublic", "Ping" };

        //        roomOptions.CustomRoomProperties
        PhotonNetwork.CreateRoom(_serverName, roomOptions);


        // Set Owner로 MyRoomManager에서 Start button을 Active 시킵니다. 다른게 더 필요하면 거기서 설정하면 됩니다.
        FindObjectOfType<MyRoomManager>().OnRoomCreateOrJoin(true);
    }
}
