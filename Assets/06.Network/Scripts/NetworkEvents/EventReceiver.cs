using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class EventReceiver : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public Dictionary<EventCode, NetworkEvent> events = new Dictionary<EventCode, NetworkEvent>();
    public override void OnEnable()
    {
        // 이벤트 콜백을 등록
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        // 이벤트 콜백 등록 해제
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    // 이벤트 수신 처리 메서드
    public void OnEvent(EventData photonEvent)
    {
        EventCode eventCode = (EventCode)photonEvent.Code;

        if(events.ContainsKey(eventCode))
            events[eventCode].OnEvent(photonEvent.CustomData);
    }
}
