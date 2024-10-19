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
                NetworkManager.Instance.StartGame(photonEvent.CustomData);
                break;
            case EventCode.AttackRequest:
                object[] datas = (object[])photonEvent.CustomData; // ���۵� �����͸� ����
                //������ : {������ID, �ǰ���ID}
                
                PhotonView from = PhotonNetwork.GetPhotonView((int)datas[0]);
                PhotonView to = PhotonNetwork.GetPhotonView((int)datas[1]);

                int fromViewID = from.ViewID;
                int toViewID = to.ViewID;



                //TODO: ������ �ǰ��� �̿��ؼ� �ؾ��ϴ� ������ ó���ϱ�
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
