using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.InputSystem;

public class VoiceManager : MonoBehaviourPunCallbacks
{
    Recorder recorder;  // 음성을 송신하는 Recorder

    private void Start()
    {
        recorder = GetComponent<Recorder>();
        recorder.enabled = (Microphone.devices.Length != 0);

        // 마이크 활성화 및 음성 전송 시작
        PunVoiceClient.Instance.ConnectAndJoinRoom();
    }

    public void PressToTalk(bool value)
    {
        recorder.enabled = (Microphone.devices.Length != 0);
        if (!recorder.enabled) return;
        if (recorder.TransmitEnabled == value) return;

        recorder.TransmitEnabled = value;
    }

}
