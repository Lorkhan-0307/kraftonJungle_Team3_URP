using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEHungerGauge : NetworkEvent
{
    protected override void Awake()
    {
        this.eventCode = EventCode.HungerGauge;
        base.Awake();
    }
    public override void OnEvent(object customData)
    {
        if (!NetworkManager.Instance.Monster) return;

        bool ishungerzero = (bool)customData;

        if (ishungerzero)
        {
            NetworkManager.Instance.Monster.OnHunger();
        }
        else
        {
            NetworkManager.Instance.Monster.NoHunger();
        }
    }
}
