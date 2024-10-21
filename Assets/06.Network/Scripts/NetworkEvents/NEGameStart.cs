using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEGameStart : NetworkEvent
{
    protected override void Awake()
    {
        this.eventCode = EventCode.GameStart;
        base.Awake();
    }
    public override void OnEvent(object customData)
    {
        NetworkManager.Instance.StartGame(customData);
    }
}
