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
    public void FixedUpdate()
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
        newData = ani.GetBool(isRunningKey);
        if (isRunning != newData)
            send = true;

        if (send)
        {
            photonView.RPC("SyncAniBools", RpcTarget.Others, BoolsToData());
        }
    }

    object BoolsToData()
    {
        object[] data = { isWalking, isRunning };
        return data;
    }

    [PunRPC]
    public void SyncAniBools(object data)
    {
        object[] datas = (object[])data;
        isWalking = (bool)datas[0];
        isRunning = (bool)datas[1];

        ani.SetBool(isWalkingKey, isWalking);
        ani.SetBool(isRunningKey, isRunning);
    }
}
