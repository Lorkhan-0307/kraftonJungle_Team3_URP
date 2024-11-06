using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class TransformSync : MonoBehaviourPunCallbacks, IPunObservable
{
    private Vector3 latestPos; // 네트워크에서 받은 최신 위치
    private Quaternion latestRot; // 네트워크에서 받은 최신 회전
    private Vector3 velocity; // 위치 변화 속도 계산용

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
            // 최신 위치와 회전 값 수신
            Vector3 previousPos = latestPos; // 이전 위치 저장
            latestPos = (Vector3)stream.ReceiveNext(); // 위치 받기
            latestRot = (Quaternion)stream.ReceiveNext(); // 회전 받기

            // 새 위치와 이전 위치 간의 속도 계산
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            velocity = (latestPos - previousPos) / lag; // 속도 계산
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) // 다른 클라이언트의 오브젝트일 때
        {
            // Extrapolation을 사용해 위치 예측
            float extrapolationTime = Mathf.Clamp(Time.deltaTime, 0, 0.5f); // 예측 시간 제한
            Vector3 extrapolatedPos = latestPos + velocity * extrapolationTime; // 예측 위치 계산


            // 예측 위치와 회전 적용
            try
            {
                transform.position = Vector3.Lerp(transform.position, extrapolatedPos, Time.deltaTime * 10); // 위치 보간
            }
            catch
            {
                Debug.Log($"{transform.position.ToString()}, {extrapolatedPos.ToString()}, : {Time.deltaTime.ToString()} Error!");
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, latestRot, Time.deltaTime * 10); // 회전 보간
        }
    }
}
