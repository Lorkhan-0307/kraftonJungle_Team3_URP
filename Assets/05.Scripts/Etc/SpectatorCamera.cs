using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SpectatorCamera : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    private List<GameObject> remainingPlayers = new List<GameObject>();
    private GameObject spectatingTarget;
    private int currentPlayerIndex = 0;

    private float xRotation = 0f;

    private PlayerInput playerInput;
    private InputAction lookAction;
    private InputAction prevPlayerAction;
    private InputAction nextPlayerAction;

    private CinemachineVirtualCamera virtualCamera;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        lookAction = playerInput.actions["Look"];
        prevPlayerAction = playerInput.actions["PrevPlayer"];
        nextPlayerAction = playerInput.actions["NextPlayer"];
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    void Start()
    {
        remainingPlayers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        Cursor.lockState = CursorLockMode.Locked;
        virtualCamera.Follow = this.transform;
    }

    // Update로 관전 대상 변경 감지, 버츄얼 카메라 위치 업데이트
    void Update()
    {
        // Handle switching between players
        if (prevPlayerAction.triggered)
        {
            SwitchPlayer(-1);
        }
        if (nextPlayerAction.triggered)
        {
            SwitchPlayer(1);
        }

        UpdateVirtualCameraPosition();
    }

    // 버츄얼 카메라 위치 업데이트
    private void UpdateVirtualCameraPosition()
    {
        this.transform.position = spectatingTarget.transform.position;
        Vector2 mouseDelta = lookAction.ReadValue<Vector2>();

        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);
    }

    // 관전 대상 변경
    private void SwitchPlayer(int idx_change)
    {
        if (remainingPlayers.Count == 0)
            return;

        currentPlayerIndex += idx_change;
        if (currentPlayerIndex < 0)
            currentPlayerIndex = remainingPlayers.Count - 1;
        else if (currentPlayerIndex >= remainingPlayers.Count)
            currentPlayerIndex = 0;

        spectatingTarget = remainingPlayers[currentPlayerIndex];
    }

    // 다른 플레이어가 죽었을 때 SpectatorManager가 호출
    public void RemovePlayer(GameObject player)
    {
        remainingPlayers.Remove(player);
        if (spectatingTarget == player)
        {
            currentPlayerIndex = currentPlayerIndex % remainingPlayers.Count;
            spectatingTarget = remainingPlayers[currentPlayerIndex];
        }
    }
}
