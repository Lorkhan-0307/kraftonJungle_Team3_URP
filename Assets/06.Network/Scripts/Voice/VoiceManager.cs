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

    // 최적화 작업
    //private void Update()
    //{
    //    if (NetworkManager.Instance.curState == GameState.Dead)
    //    {
    //        return;
    //    }
    //    recorder.enabled = (Microphone.devices.Length != 0);
    //}

    public void PressToTalk(bool value)
    {
        recorder.enabled = (Microphone.devices.Length != 0);
        if (SpectatorManager.instance.isSpectating)
        {
            recorder.TransmitEnabled = false;
            return;
        }
        if (!recorder.enabled) return;
        if (recorder.TransmitEnabled == value) return;

        recorder.TransmitEnabled = value;
    }

}
