using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpectateCamera : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform[] playerBodys;
    public Transform cameraBody;
    float xRotation = 0f;

    // Input Actions 변수
    private PlayerInput playerInput;
    private InputAction lookAction;
    private InputAction changeSpectatePlayerAction;

    private int currentPlayerNum = 0;

    void Awake()
    {
        // PlayerInput 컴포넌트에서 InputAction 가져오기
        playerInput = GetComponent<PlayerInput>();
        lookAction = playerInput.actions["Look"];
        //playerBody = GetComponentInParent<CharacterController>().transform;

        changeSpectatePlayerAction = playerInput.actions["ChangeSpectate"];
        
    }
    
    

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (changeSpectatePlayerAction.triggered)
        {
            currentPlayerNum += 1;
            if (currentPlayerNum >= playerBodys.Length) currentPlayerNum = 0;
        }
        this.transform.position = playerBodys[currentPlayerNum].transform.position;
        // Look 액션으로 마우스 움직임 입력 받기
        Vector2 mouseDelta = lookAction.ReadValue<Vector2>();

        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        // 상하 회전 제한
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // 카메라 상하 회전
        //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        // 플레이어 좌우 회전
        cameraBody.Rotate(Vector3.up * mouseX);
    }
}