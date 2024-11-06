using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadEventReceiver : MonoBehaviour
{
    public void OnAniEnd()
    {
        LoadingManager.instance.isAniEnded = false;
    }
}
