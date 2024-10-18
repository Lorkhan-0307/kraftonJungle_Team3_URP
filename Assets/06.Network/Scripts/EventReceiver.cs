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
            case EventCode.AttackToServer:
                string message = (string)photonEvent.CustomData; // 전송된 데이터를 받음
                Debug.Log("이벤트 수신: " + message);
                string[] tokens = message.Split(',');
                PhotonView from = PhotonNetwork.GetPhotonView(int.Parse(tokens[0]));
                PhotonView to = PhotonNetwork.GetPhotonView(int.Parse(tokens[1]));
                ServerLogic.Instance.HitScan(from.transform, to.transform);
                break;
        }
    }
}
