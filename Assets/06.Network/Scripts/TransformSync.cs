using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class TransformSync : MonoBehaviourPunCallbacks, IPunObservable
{
    private Vector3 latestPos; // 네트워크에서 동기화된 위치
    private Quaternion latestRot; // 네트워크에서 동기화된 회전

    // 네트워크 동기화를 위한 메서드
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // 내가 데이터를 전송할 때
        {
            stream.SendNext(transform.position); // 위치 전송
            stream.SendNext(transform.rotation); // 회전 전송
        }
        else // 다른 플레이어의 데이터를 받을 때
        {
            latestPos = (Vector3)stream.ReceiveNext(); // 위치 받기
            latestRot = (Quaternion)stream.ReceiveNext(); // 회전 받기
        }
    }
    private void Update()
    {
        if (!photonView.IsMine) // 다른 클라이언트의 오브젝트일 때
        {
            // 위치와 회전을 부드럽게 업데이트
            transform.position = Vector3.Lerp(transform.position, latestPos, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, latestRot, Time.deltaTime * 10);
        }
    }
}
