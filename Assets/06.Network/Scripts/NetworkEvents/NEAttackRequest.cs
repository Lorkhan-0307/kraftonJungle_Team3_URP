using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEAttackRequest : NetworkEvent
{
    protected override void Awake()
    {
        this.eventCode = EventCode.AttackRequest;
        base.Awake();
    }
    public override void OnEvent(object customData)
    {
        object[] datas = (object[])customData; // 전송된 데이터를 받음
                                                           //데이터 : {공격자ID, 피격자ID}

        PhotonView from = PhotonNetwork.GetPhotonView((int)datas[0]);
        PhotonView to = PhotonNetwork.GetPhotonView((int)datas[1]);

        if (!from) return;
        if (!to) return;

        //TODO: 공격자 피격자 이용해서 해야하는 로직들 처리하기
        to.GetComponent<Player>().OnDamaged(from.gameObject);
    }

    public static void AttackEntity(PhotonView from, PhotonView to)
    {
        object[] result = { from.ViewID, to.ViewID };

        NetworkManager.SendToClients(EventCode.AttackRequest, result);
    }
}
