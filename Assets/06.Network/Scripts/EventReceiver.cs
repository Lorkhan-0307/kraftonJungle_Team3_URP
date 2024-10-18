using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;

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

                //TODO: 자신의 플레이어 ActorNumber 가 전송받은 id와 같은지 비교하고 몬스터, 연구원으로 초기화함.
                if(PhotonNetwork.LocalPlayer.ActorNumber == id)
                {

                }
                break;
            case EventCode.AttackRequest:
                string message = (string)photonEvent.CustomData; // 전송된 데이터를 받음
                //데이터 : "{공격자ID},{피격자ID}"
                Debug.Log("이벤트 수신: " + message);
                string[] tokens = message.Split(',');
                PhotonView from = PhotonNetwork.GetPhotonView(int.Parse(tokens[0]));
                PhotonView to = PhotonNetwork.GetPhotonView(int.Parse(tokens[1]));

                break;
            case EventCode.PlayerDeath:

                break;
            case EventCode.EndGame:

                break;
            case EventCode.SwitchDayNight:

                break;
        }
    }
}
