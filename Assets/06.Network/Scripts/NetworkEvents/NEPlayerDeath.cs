using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEPlayerDeath : NetworkEvent
{
    protected override void Awake()
    {
        this.eventCode = EventCode.PlayerDeath;
        base.Awake();
    }
    public override void OnEvent(object customData)
    {
        if (NetworkManager.Instance.IsServer())
        {
            ServerLogic server = FindObjectOfType<ServerLogic>();
            server.PlayerDeath((int)customData);
        }
    }


    /// <summary>
    /// 죽은 플레이어 본인이 죽은 시점에 실행시켜주세요.
    /// Run by the dead player.
    /// </summary>
    public static void PlayerDeath()
    {
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        NetworkManager.Instance.curState = GameState.Dead;

        NetworkManager.SendToClients(EventCode.PlayerDeath, actorNumber);
    }
}
