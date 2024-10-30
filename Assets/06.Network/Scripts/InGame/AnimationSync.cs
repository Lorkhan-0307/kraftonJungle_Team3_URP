using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class AnimationSync : MonoBehaviourPun
{
    bool isWalking = false;
    bool isRunning = false;

    string isWalkingKey = "IsWalking";
    string isRunningKey = "IsRunning";
    public Animator ani;


    public void SyncAniTrigger(string triggerName)
    {
        // 로컬 오브젝트가 아닐 때만 동기화
        if (!photonView.AmOwner)
            ani.SetTrigger(triggerName);
    }

    // 네트워크 동기화를 위한 메서드
    public void Update()
    {
        SendBools();
    }

    void SendBools()
    {
        // 전송 부
        if (!photonView.AmOwner) return;

        bool send = false;

        bool newData = ani.GetBool(isWalkingKey);
        if (isWalking != newData)
            send = true;
        isWalking = newData;

        newData = ani.GetBool(isRunningKey);
        if (isRunning != newData)
            send = true;
        isRunning = newData;

        if (send)
        {
            Debug.Log("SendANI!");
            photonView.RPC("SyncAniBools", RpcTarget.Others, BoolsToData());
        }
    }

    object BoolsToData()
    {
        return new object[] { isWalking, isRunning };
    }

    [PunRPC]
    public void SyncAniBools(object data)
    {
        object[] datas = (object[])data;
        isWalking = (bool)datas[0];
        isRunning = (bool)datas[1];

        Debug.Log("ReceiveANI!");

        ani.SetBool(isWalkingKey, isWalking);
        ani.SetBool(isRunningKey, isRunning);
    }
}
