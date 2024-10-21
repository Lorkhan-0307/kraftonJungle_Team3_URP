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
        GameManager.instance.SwitchTime(isDay);
        FindObjectOfType<TimeSwitchSlider>().SyncTime(0);
    }
}
