using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEOnDisconnected : NetworkEvent
{
    protected override void Awake()
    {
        this.eventCode = EventCode.OnDisconnected;
        base.Awake();
    }
    public override void OnEvent(object customData)
    {
        object[] datas = (object[])customData;

        int actorNum = (int)datas[0];
        int viewID = (int)datas[1];

        // TODO: 팅긴 유저와 해당 유저의 캐릭터 소유권 양도 받았음.
        // TODO: 해당 유저에 팅김 처리, 아이디 저장
    }

    // 서버에게 이벤트를 보냄.
    public static void HungerEvent(int viewID)
    {
        object[] content = { PhotonNetwork.LocalPlayer.ActorNumber, viewID };
        NetworkManager.SendToServer(EventCode.OnDisconnected, content);
    }
}
