using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class TransformSync : MonoBehaviourPunCallbacks, IPunObservable
{
    private Vector3 latestPos; // 네트워크에서 받은 최신 위치
    private Quaternion latestRot; // 네트워크에서 받은 최신 회전
    private Vector3 velocity; // 위치 변화 속도 계산용
    private Vector3 lastValidPosition;

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
            //Vector3 previousPos = latestPos; // 이전 위치 저장
            //latestPos = (Vector3)stream.ReceiveNext(); // 위치 받기

            Vector3 receivedPosition = (Vector3)stream.ReceiveNext();

            // 수신된 위치 값 검증
            if (IsValidPosition(receivedPosition))
            {
                latestPos = receivedPosition;
                lastValidPosition = receivedPosition;
                latestRot = (Quaternion)stream.ReceiveNext(); // 회전 받기
            }
            else
            {
                Debug.LogWarning($"Received invalid position: {receivedPosition}");
            }

            // 새 위치와 이전 위치 간의 속도 계산
            //float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            //velocity = (latestPos - previousPos) / lag; // 속도 계산
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) // 다른 클라이언트의 오브젝트일 때
        {
            // 1. 위치 값 유효성 검사
            if (IsValidPosition(latestPos))
            {
                // 부드러운 보간 이동
                transform.position = Vector3.Lerp(transform.position, latestPos, Time.deltaTime * 10f);
                transform.rotation = Quaternion.Lerp(transform.rotation, latestRot, Time.deltaTime * 10f);
            }
            else
            {
                Debug.LogWarning($"Invalid position detected: {latestPos}");
                // 문제가 있는 경우 마지막으로 알려진 유효한 위치로 리셋
                ResetToLastValidPosition();
            }

            //float deltaTime = Time.smoothDeltaTime;
            //// Extrapolation을 사용해 위치 예측
            //float extrapolationTime = Mathf.Clamp(deltaTime, 0, 0.5f); // 예측 시간 제한
            //Vector3 extrapolatedPos = latestPos + velocity * extrapolationTime; // 예측 위치 계산


            //// 예측 위치와 회전 적용
            //try
            //{
            //    transform.position = Vector3.Lerp(transform.position, extrapolatedPos, deltaTime * 10); // 위치 보간
            //}
            //catch
            //{
            //    Debug.Log($"{transform.position.ToString()}, {extrapolatedPos.ToString()}, : {deltaTime.ToString()} Error!");
            //}
            //transform.rotation = Quaternion.Lerp(transform.rotation, latestRot, deltaTime * 10); // 회전 보간
        }
    }

    private void ResetToLastValidPosition()
    {
        // 마지막으로 알려진 유효한 위치로 리셋
        transform.position = lastValidPosition;
        latestPos = lastValidPosition;
    }

    // 위치 값 유효성 검사 함수
    private bool IsValidPosition(Vector3 position)
    {
        // 무한대 값 체크
        if (float.IsInfinity(position.x) || float.IsInfinity(position.y) || float.IsInfinity(position.z))
            return false;

        // NaN 값 체크
        if (float.IsNaN(position.x) || float.IsNaN(position.y) || float.IsNaN(position.z))
            return false;

        // 너무 큰 값 체크 (게임에 따라 적절한 범위 설정)
        float maxAllowedPosition = 1000f; // 예시 값
        if (Mathf.Abs(position.x) > maxAllowedPosition ||
            Mathf.Abs(position.y) > maxAllowedPosition ||
            Mathf.Abs(position.z) > maxAllowedPosition)
            return false;

        return true;
    }

}
