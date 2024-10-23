using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpectatorCamera : MonoBehaviour
{
    public GameObject SpectatingTarget;               // 카메라가 따라다닐 타겟

    public float offsetX = 0f;            // 카메라의 x좌표
    public float offsetY = 1.0f;           // 카메라의 y좌표
    public float offsetZ = 0f;          // 카메라의 z좌표

    public float CameraSpeed = 10.0f;       // 카메라의 속도
    Vector3 TargetPos;                      // 타겟의 위치

    public void SetSpectatingTarget(GameObject target)
    {
        SpectatingTarget = target;
        gameObject.GetComponentInChildren<MouseComponent>().playerBody = gameObject.transform;
        // gameObject.GetComponent<MouseComponent>().playerBody = target.transform;
        Debug.Log("Set Spectating Target: " + target.name);
    }

    private void FixedUpdate()
    {
        // 타겟의 x, y, z 좌표에 카메라의 좌표를 더하여 카메라의 위치를 결정
        TargetPos = new Vector3(
            SpectatingTarget.transform.position.x + offsetX,
            SpectatingTarget.transform.position.y + offsetY,
            SpectatingTarget.transform.position.z + offsetZ
            );

        transform.position = TargetPos;

        // 카메라의 움직임을 부드럽게 하는 함수(Lerp)
        // transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
    }
}