using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;

public class EventReceiver : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private void OnEnable()
    {
        // �̺�Ʈ �ݹ��� ���
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        // �̺�Ʈ �ݹ� ��� ����
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    // �̺�Ʈ ���� ó�� �޼���
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        switch((EventCode)eventCode)
        {
            case EventCode.GameStart:
                int id = (int)photonEvent.CustomData;

                //TODO: �ڽ��� �÷��̾� ActorNumber �� ���۹��� id�� ������ ���ϰ� ����, ���������� �ʱ�ȭ��.
                if(PhotonNetwork.LocalPlayer.ActorNumber == id)
                {

                }
                break;
            case EventCode.AttackRequest:
                string message = (string)photonEvent.CustomData; // ���۵� �����͸� ����
                //������ : "{������ID},{�ǰ���ID}"
                Debug.Log("�̺�Ʈ ����: " + message);
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
