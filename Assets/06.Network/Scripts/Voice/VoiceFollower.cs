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
            // 리모트 캐릭터 따라가기
            //List<Player> players = FindObjectsOfType<Player>().ToList();
            //Player myPlayer = players.Find(x => (x.GetComponent<PhotonView>().Owner == 
            //PhotonNetwork.CurrentRoom.GetPlayer(GetComponent<Speaker>().RemoteVoice.PlayerId)
            //&& x.type != CharacterType.NPC));
            List<Player> players = FindObjectsOfType<Player>().ToList();
            Player myPlayer = players.Find(x => x.GetComponent<PhotonView>().
            Owner.ActorNumber == GetComponent<Speaker>().RemoteVoice.PlayerId);

            if (!myPlayer)
                return;
                
            target = myPlayer.transform;
            //Debug.Log("Set PARENT Voice");

            transform.SetParent(target.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

        }
    }
}
