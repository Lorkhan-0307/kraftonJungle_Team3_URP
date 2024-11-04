using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NEOnSceneLoaded : NetworkEvent
{
    protected override void Awake()
    {
        this.eventCode = EventCode.OnSceneLoaded;
        base.Awake();
    }

    ServerLogic Server {
        get
        {
            if(!server) server = FindObjectOfType<ServerLogic>();
            return server;
        }
    }
    ServerLogic server = null;
    public override void OnEvent(object customData)
    {
        int actorNum = (int)customData;

        // TODO: 해당 캐릭터가 접속되었음. 모든 캐릭터가 접속되었을 시 게임 실행
        if (!Server) return;
        Server.PlayerSceneLoaded(actorNum);
    }

    // 서버에게 이벤트를 보냄.
    public static void SceneLoaded()
    {
        object content = PhotonNetwork.LocalPlayer.ActorNumber;
        NetworkManager.SendToServer(EventCode.OnSceneLoaded, content);
    }
}
