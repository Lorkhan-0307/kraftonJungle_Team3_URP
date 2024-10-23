using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpectatorCamera : MonoBehaviour
{
    public GameObject SpectatingTarget;               // 카메라가 따라다닐 타겟
    public float mouseSensitivity = 100f;   // 마우스 감도
    Vector3 TargetPos;                      // 타겟의 위치

    // Input Actions 변수
    private PlayerInput playerInput;
    private InputAction lookAction;

    void Awake()
    {
        // PlayerInput 컴포넌트에서 InputAction 가져오기
        playerInput = GetComponent<PlayerInput>();
        lookAction = playerInput.actions["Look"];

        Vector3 virtualCameraPosition = transform.Find("Virtual Camera").transform.position;
        transform.Find("Virtual Camera").transform.position = virtualCameraPosition;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetSpectatingTarget(GameObject target)
    {
        SpectatingTarget = target;
        Debug.Log("Set Spectating Target: " + target.name);
    }

    private void FollowPosition()
    {
        transform.position = SpectatingTarget.transform.position;
        Vector2 mouseDelta = lookAction.ReadValue<Vector2>();

        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }

    private void FixedUpdate()
    {
        if (SpectatingTarget != null)
        {
            FollowPosition();
        }
    }
}