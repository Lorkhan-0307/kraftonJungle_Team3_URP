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
                int id = (int)photonEvent.CustomData;

                //TODO: 이거 지금 자기자신만 수정하는거같은데 이후에 다른 유저 캐릭터도 수정해줘야할듯
                //자신의 플레이어 ActorNumber 가 전송받은 id와 같은지 비교하고 몬스터, 연구원으로 초기화함.
                bool isMonster = PhotonNetwork.LocalPlayer.ActorNumber == id;

                break;
            case EventCode.AttackRequest:
                string message = (string)photonEvent.CustomData; // 전송된 데이터를 받음
                //데이터 : "{공격자ID},{피격자ID}"
                Debug.Log("이벤트 수신: " + message);
                string[] tokens = message.Split(',');
                PhotonView from = PhotonNetwork.GetPhotonView(int.Parse(tokens[0]));
                PhotonView to = PhotonNetwork.GetPhotonView(int.Parse(tokens[1]));

                //TODO: 공격자 피격자 이용해서 해야하는 로직들 처리하기
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
                bool isDay = (bool)photonEvent.CustomData;
                LightShifter shifter = FindObjectOfType<LightShifter>();
                if (isDay)
                {
                    shifter.OnDayShift();
                }
                else
                {
                    shifter.OnNightShift();
                }
                // TODO: DayNight Slider 측에도 정보 보내줘야함.


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
