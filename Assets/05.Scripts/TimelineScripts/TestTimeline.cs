using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Cinemachine;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TestTimeline : MonoBehaviour
{

    
    [SerializeField] private PlayableDirector myKillPlayableDirector;
    [SerializeField] private PlayableDirector yourKilPlayableDirector;

    public void OnTimelineStart(InputAction.CallbackContext context)
    {
        Debug.Log("MYKILL");
        myKillPlayableDirector.Play();
    }

    public void OnTimelineNotMyKillStart(InputAction.CallbackContext context)
    {
        Debug.Log("NOTMYKILL");
        yourKilPlayableDirector.Play();
    }
    
    
}
