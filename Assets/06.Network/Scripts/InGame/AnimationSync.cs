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
    [SerializeField] Animator ani;

    // 네트워크 동기화를 위한 메서드
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // 내가 데이터를 전송할 때
        {
            isWalking = ani.GetBool(isWalkingKey);
            isRunning = ani.GetBool(isRunningKey);

            stream.SendNext(isWalking); // 위치 전송
            stream.SendNext(isRunning); // 회전 전송
        }
        else // 다른 플레이어의 데이터를 받을 때
        {
            // 걷기 애니메이션
            bool newData = (bool)stream.ReceiveNext();
            if (isWalking != newData)
            {
                ani.SetBool(isWalkingKey, newData);
            }
            isWalking = newData;

            // 달리기 애니메이션
            newData = (bool)stream.ReceiveNext();
            if (isRunning != newData)
            {
                ani.SetBool(isRunningKey, newData);
            }
            isRunning = newData;
        }
    }

    public void SyncAniTrigger(string triggerName)
    {
        // 로컬 오브젝트가 아닐 때만 동기화
        if(!photonView.AmOwner)
            ani.SetTrigger(triggerName);
    }
}
