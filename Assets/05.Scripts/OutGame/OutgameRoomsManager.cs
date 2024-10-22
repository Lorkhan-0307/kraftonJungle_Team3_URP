using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutgameRoomsManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject serverButton;
    [SerializeField] private Transform serverList;

    // Todo : 서버에서 방 목록을 여기서 받아와서 작업해야 합니다.
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Server에서 public으로 오픈된 모든 방들을 로딩합니다.
        // foreach를 통해서, 각각을 AddServerButtonOnServerList 함수를 실행하면 됩니다.
        ServerButton[] buttons = serverList.GetComponentsInChildren<ServerButton>();

        Debug.Log("Destroy All");
        for (int i = 0; i < buttons.Length; i++)
        {
            Destroy(buttons[i].gameObject);
        }


        foreach (RoomInfo room in roomList)
        {
            // 방 설정에 IsPublic이 없거나 Private로 설정되어있을 경우
            if (!room.CustomProperties.ContainsKey("IsPublic") ||
                (bool)room.CustomProperties["IsPublic"] == false)
                continue;

            ServerButtonElements buttonElement = new ServerButtonElements(room);
            
            AddServerButtonOnServerList(buttonElement);
        }
    }


    // 인자로 데이터를 넘겨줘야 합니다. 
    // 인자 목록
    // 1. 서버 이름 -> Server Title
    // 2. 서버 참여 인원 / 전체 참여 가능 인원 -> serverPlayerNum / serverTotalPlayerNum
    // 3. 서버 주인 -> Server Owner
    // 4. Ping -> ping
    // 자세한 사항은 ServerButtonElements를 찾아보세요.
    private void AddServerButtonOnServerList(ServerButtonElements sbe)
    {
        Instantiate(serverButton, serverList).GetComponent<ServerButton>().SetupServerButton(sbe);
    }
}
 