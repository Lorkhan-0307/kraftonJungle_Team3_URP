using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

public class VoiceManager : MonoBehaviourPunCallbacks
{
    Recorder recorder;  // 음성을 송신하는 Recorder
    private void Start()
    {

        AudioSource audioSource = null; GetComponent<AudioSource>();
        if(audioSource != null)
        {
            audioSource.volume = 10.0f;
        }

        recorder = GetComponent<Recorder>();

        // 마이크 활성화 및 음성 전송 시작
        recorder.TransmitEnabled = (Microphone.devices.Length != 0);
        PunVoiceClient.Instance.ConnectAndJoinRoom();
    }
}
