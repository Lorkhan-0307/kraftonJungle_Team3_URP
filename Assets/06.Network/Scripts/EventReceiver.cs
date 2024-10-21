using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class EventReceiver : MonoBehaviourPunCallbacks, IOnEventCallback
{
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
        byte eventCode = photonEvent.Code;

        switch((EventCode)eventCode)
        {
            case EventCode.GameStart:
                NetworkManager.Instance.StartGame(photonEvent.CustomData);
                break;
            case EventCode.AttackRequest:
                object[] datas = (object[])photonEvent.CustomData; // 전송된 데이터를 받음
                //데이터 : {공격자ID, 피격자ID}
                
                PhotonView from = PhotonNetwork.GetPhotonView((int)datas[0]);
                PhotonView to = PhotonNetwork.GetPhotonView((int)datas[1]);

                if (!from) break;
                if (!to) break;

                //TODO: 공격자 피격자 이용해서 해야하는 로직들 처리하기
                to.GetComponent<Player>().OnDamaged(from.gameObject);

                break;
            case EventCode.PlayerDeath:
                if(NetworkManager.Instance.IsServer())
                {
                    ServerLogic server = GetComponent<ServerLogic>();
                    server.PlayerDeath((int)photonEvent.CustomData);
                }
                break;
            case EventCode.EndGame:
                bool result = EndGame((string)photonEvent.CustomData);
                if(result)
                {
                    Debug.Log("Victory!");
                }
                else
                {
                    Debug.Log("Defeat!");
                }
                break;
            case EventCode.SwitchDayNight:
                bool isDay = (bool)photonEvent.CustomData;
                Debug.Log($"SwitchDayNight {isDay}");
                GameManager.instance.SwitchTime(isDay);
                FindObjectOfType<TimeSwitchSlider>().SyncTime(0);
                break;
            case EventCode.HungerGauge:
                if (!NetworkManager.Instance.Monster) return;

                bool ishungerzero = (bool)photonEvent.CustomData;

                if(ishungerzero)
                {
                    NetworkManager.Instance.Monster.OnHunger();
                }
                else
                {
                    NetworkManager.Instance.Monster.NoHunger(); 
                }
                break;
            case EventCode.AccelTime:
                float SkipTime = (float)photonEvent.CustomData;

                FindObjectOfType<TimeSwitchSlider>().SyncTime(SkipTime);
                break;
        }
    }

    public bool EndGame(string data)
    {
        string[] tokens = data.Split(",");
        int myID = PhotonNetwork.LocalPlayer.ActorNumber;
        for(int i = 0; i < tokens.Length; i++)
        {
            if (myID == int.Parse(tokens[i]))
                return true;
        }

        return false;
    }

}
