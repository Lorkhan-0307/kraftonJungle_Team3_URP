using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpectatorCamera : MonoBehaviour
{
    public GameObject SpectatingTarget;               // 카메라가 따라다닐 타겟

    [SerializeField] private float offsetX = 0f;            // 카메라의 x좌표
    [SerializeField] private float offsetY = 1.0f;           // 카메라의 y좌표
    [SerializeField] private float offsetZ = 0f;          // 카메라의 z좌표
    [SerializeField] private float distance = 1.5f;       // 카메라와 타겟 사이 평면 거리
    [SerializeField] private float height = 1.0f;         // 카메라의 높이


    public float CameraSpeed = 10.0f;       // 카메라의 속도
    Vector3 TargetPos;                      // 타겟의 위치

    public void SetSpectatingTarget(GameObject target)
    {
        SpectatingTarget = target;
        gameObject.GetComponentInChildren<MouseComponent>().playerBody = gameObject.transform;
        // gameObject.GetComponent<MouseComponent>().playerBody = target.transform;
        Debug.Log("Set Spectating Target: " + target.name);
    }

    private void FollowPosition()
    {
        // TargetPos = new Vector3(
        //     SpectatingTarget.transform.position.x + offsetX,
        //     SpectatingTarget.transform.position.y + offsetY,
        //     SpectatingTarget.transform.position.z + offsetZ
        //     );

        float angle = SpectatingTarget.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float cosine = Mathf.Cos(angle);
        float sine = Mathf.Sin(angle);

        TargetPos = new Vector3(
            SpectatingTarget.transform.position.x - distance * cosine,
            SpectatingTarget.transform.position.y + height,
            SpectatingTarget.transform.position.z - distance * sine
            );

        transform.position = TargetPos;

        // 카메라의 움직임을 부드럽게 하는 함수(Lerp)
        // transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);

    }

    private void FixedUpdate()
    {
        if (SpectatingTarget != null)
        {
            FollowPosition();
        }
    }
}