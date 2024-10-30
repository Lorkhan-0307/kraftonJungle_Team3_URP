using Photon.Pun;
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
        object[] datas = (object[])customData;
        bool ishungerzero = (bool)datas[0];
        int monsterNum = (int)datas[1];


        if (!NetworkManager.Instance.Monsters.ContainsKey(monsterNum)) return;

        if (ishungerzero)
        {
            NetworkManager.Instance.Monsters[monsterNum].OnHunger();
        }
        else
        {
            NetworkManager.Instance.Monsters[monsterNum].NoHunger();
        }
    }

    // 모든 클라이언트에게 이벤트를 보냄.
    public static void HungerEvent(bool ishungerzero)
    {
        NetworkManager.SendToClients(EventCode.HungerGauge, new object[] 
            { ishungerzero, PhotonNetwork.LocalPlayer.ActorNumber});
    }

}
