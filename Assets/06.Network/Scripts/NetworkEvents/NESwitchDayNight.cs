using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NESwitchDayNight : NetworkEvent
{
    protected override void Awake()
    {
        this.eventCode = EventCode.SwitchDayNight;
        base.Awake();
    }
    public override void OnEvent(object customData)
    {
        bool isDay = (bool)customData;
        Debug.Log($"SwitchDayNight {isDay}");
        TimeManager.instance.SwitchTime(isDay);
        TimeManager.instance.SyncTime(0);
    }

    /// <summary>
    /// 0: Day, 1:Night
    /// </summary>
    public static void SwitchDayNight(bool isDay)
    {
        NetworkManager.SendToClients(EventCode.SwitchDayNight, isDay);
    }
}
