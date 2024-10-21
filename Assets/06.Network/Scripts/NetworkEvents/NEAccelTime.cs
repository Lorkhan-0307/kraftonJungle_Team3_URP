using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEAccelTime : NetworkEvent
{
    protected override void Awake()
    {
        this.eventCode = EventCode.AccelTime;
        base.Awake();
    }
    public override void OnEvent(object customData)
    {
        float SkipTime = (float)customData;

        FindObjectOfType<TimeSwitchSlider>().SyncTime(SkipTime);
    }

    public static void TimeAccel(float SkipTime)
    {
        NetworkManager.SendToClients(EventCode.AccelTime, SkipTime);
    }
}
