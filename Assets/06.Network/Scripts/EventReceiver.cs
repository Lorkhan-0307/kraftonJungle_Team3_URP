using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using System;

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

                //TODO: ������ �ǰ��� �̿��ؼ� �ؾ��ϴ� ������ ó���ϱ�
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
                // TODO: ��� Ŭ���̾�Ʈ���� ���� �ð� ���

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
