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
        recorder = GetComponent<Recorder>();

        recorder.enabled = (Microphone.devices.Length != 0);

        AudioSource audioSource = GetComponent<AudioSource>();
        if(audioSource != null)
        {
            audioSource.volume = 10.0f;
            audioSource.panStereo = 2.0f;

            AudioReverbFilter reverbfilter = audioSource.gameObject.AddComponent<AudioReverbFilter>();
            reverbfilter.reverbPreset = AudioReverbPreset.Cave;
            reverbfilter.dryLevel = 0;
            reverbfilter.reverbLevel = 1;
        }

        // 마이크 활성화 및 음성 전송 시작
        PunVoiceClient.Instance.ConnectAndJoinRoom();
    }
}
