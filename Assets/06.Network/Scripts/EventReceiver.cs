using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using System;

public class EventReceiver : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private void OnEnable()
    {
        // 이벤트 콜백을 등록
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
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

                int fromViewID = from.ViewID;
                int toViewID = to.ViewID;



                //TODO: 공격자 피격자 이용해서 해야하는 로직들 처리하기
                if(to.AmOwner)
                {
                    to.GetComponent<Player>().OnDamaged(from.gameObject);
                }
                break;
            case EventCode.PlayerDeath:
                ServerLogic server = GetComponent<ServerLogic>();
                if(server)
                {
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

                break;
            case EventCode.HungerGauge:
                bool ishungerzero = (bool)photonEvent.CustomData;

                if(ishungerzero)
                {
                    NetworkManager.Instance.monster.OnHunger();
                }
                else
                {
                    NetworkManager.Instance.monster.NoHunger(); 
                }
                break;
            case EventCode.AccelTime:
                NetworkManager.Instance.timeswitchslider.FastTime();
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
