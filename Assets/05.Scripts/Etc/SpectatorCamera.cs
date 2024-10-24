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
        remainingPlayers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));

        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        virtualCamera.Follow = this.transform;
    }

    // 관전 대상 리스트 초기화
    public void SetRemainingPlayers(List<GameObject> players)
    {
        remainingPlayers = players;
        if (remainingPlayers.Count > 0)
        {
            currentPlayerIndex = 0;
            spectatingTarget = remainingPlayers[currentPlayerIndex];
        }
        else
        {
            Debug.LogWarning("No players left to spectate.");
        }
    }

    // Update로 관전 대상 변경 감지, 버츄얼 카메라 위치 업데이트
    void Update()
    {
        // Handle switching between players
        if (prevPlayerAction.triggered)
        {
            SwitchToPrevPlayer();
        }
        if (nextPlayerAction.triggered)
        {
            SwitchToNextPlayer();
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
    private void SwitchToPrevPlayer()
    {
        if (remainingPlayers.Count == 0)
            return;

        currentPlayerIndex--;
        if (currentPlayerIndex < 0)
            currentPlayerIndex = remainingPlayers.Count - 1;

        spectatingTarget = remainingPlayers[currentPlayerIndex];
    }

    private void SwitchToNextPlayer()
    {
        if (remainingPlayers.Count == 0)
            return;

        currentPlayerIndex++;
        if (currentPlayerIndex >= remainingPlayers.Count)
            currentPlayerIndex = 0;

        spectatingTarget = remainingPlayers[currentPlayerIndex];
    }

    // 어떤 플레이어가 죽었을 때 SpectatorManager가 호출
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
