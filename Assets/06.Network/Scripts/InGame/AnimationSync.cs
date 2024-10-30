using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class AnimationSync : MonoBehaviourPunCallbacks, IPunObservable
{
    bool isWalking = false;
    bool isRunning = false;

    string isWalkingKey = "IsWalking";
    string isRunningKey = "IsRunning";
    public Animator ani;

    // 네트워크 동기화를 위한 메서드
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // 내가 데이터를 전송할 때
        {
            bool send = false;

            bool newData = ani.GetBool(isWalkingKey);
            if (isWalking != newData)
                send = true;
            newData = ani.GetBool(isRunningKey);
            if (isRunning != newData)
                send = true;

            if(send)
            {
                stream.SendNext(isWalking); // 위치 전송
                stream.SendNext(isRunning); // 회전 전송
            }
        }
        else // 다른 플레이어의 데이터를 받을 때
        {
            // 걷기 애니메이션
            isWalking = (bool)stream.ReceiveNext();
            // 달리기 애니메이션
            isRunning = (bool)stream.ReceiveNext();
        }
    }

    public void SyncAniTrigger(string triggerName)
    {
        // 로컬 오브젝트가 아닐 때만 동기화
        if(!photonView.AmOwner)
            ani.SetTrigger(triggerName);
    }
}
