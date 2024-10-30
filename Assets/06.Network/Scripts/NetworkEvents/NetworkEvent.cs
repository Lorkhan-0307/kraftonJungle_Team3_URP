using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkEvent : MonoBehaviour
{
    [HideInInspector]
    public EventCode eventCode;
    protected virtual void Awake()
    {
        if(!GetComponent<EventReceiver>().events.ContainsKey(eventCode))
            GetComponent<EventReceiver>().events.Add(eventCode, this);
    }

    public abstract void OnEvent(object customData);

    public virtual void Init()
    {

    }
}