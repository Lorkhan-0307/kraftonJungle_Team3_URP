using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : NetworkEvent
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
}
