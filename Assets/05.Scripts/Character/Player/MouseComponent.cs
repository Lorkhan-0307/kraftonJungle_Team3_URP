using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseComponent : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;


    // Input Actions 변수
    private PlayerInput playerInput;
    private InputAction lookAction;

    private PlayerMovement pm;

    void Awake()
    {
        // PlayerInput 컴포넌트에서 InputAction 가져오기
        playerInput = GetComponent<PlayerInput>();
        lookAction = playerInput.actions["Look"];
        playerBody = GetComponentInParent<CharacterController>().transform;
        pm = FindObjectOfType<PlayerMovement>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // 끝났으면 마우스로 시점 조종 불가
        if (TimeManager.instance.isEnd)
        {
            this.enabled = false;
            return;
        }

        if (pm.isMovable)
        {
            // Look 액션으로 마우스 움직임 입력 받기
            Vector2 mouseDelta = lookAction.ReadValue<Vector2>();

            float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
            float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

            // 상하 회전 제한
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 60f);

            // 카메라 상하 회전
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // 플레이어 좌우 회전
            playerBody.Rotate(Vector3.up * mouseX);
        }

    }
}