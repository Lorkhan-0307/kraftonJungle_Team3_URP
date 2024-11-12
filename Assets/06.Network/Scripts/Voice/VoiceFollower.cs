using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VoiceFollower : MonoBehaviourPun
{
    Transform target = null;

    private void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.volume = 10.0f;

            AudioReverbFilter reverbfilter = audioSource.gameObject.AddComponent<AudioReverbFilter>();
            reverbfilter.reverbPreset = AudioReverbPreset.Room;
            reverbfilter.dryLevel = 0;
            reverbfilter.reverbLevel = 1;
        }
    }
    private void Update()
    {
        if (target == null)
        {
            if (Camera.main != null)
            {
                target = Camera.main.transform;
            }
        }

        Follow();
    }

    void Follow()
    {
        if (target == null) return;

        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
