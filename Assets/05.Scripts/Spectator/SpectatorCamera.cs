// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using Cinemachine;

// public class SpectatorCamera : MonoBehaviour
// {
//     public float mouseSensitivity = 50f;
//     private List<GameObject> remainingPlayers = new List<GameObject>();
//     private GameObject spectatingTarget;
//     private int currentPlayerIndex = 0;

//     private float xRotation = 0f;

//     private PlayerInput playerInput;
//     private InputAction lookAction;
//     private InputAction prevPlayerAction;
//     private InputAction nextPlayerAction;

//     private CinemachineVirtualCamera virtualCamera;

//     [SerializeField] private GameObject canvasPrefab;

//     private GameObject canvasInstance;
//     private SpectatorText spectatorText;

//     void Awake()
//     {
//         playerInput = GetComponent<PlayerInput>();
//         lookAction = playerInput.actions["Look"];
//         prevPlayerAction = playerInput.actions["PrevPlayer"];
//         nextPlayerAction = playerInput.actions["NextPlayer"];
//         virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
//         Debug.Log("Spectator Camera Awake");
//     }

//     void Start()
//     {
//         remainingPlayers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player")); // 모든 플레이어를 찾아서 리스트에 추가
//         Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 숨김
//         spectatingTarget = remainingPlayers[currentPlayerIndex]; // 초기 관전 대상 설정
//         virtualCamera.Follow = this.transform; // 버츄얼 카메라가 Spectator를 따라다니도록 설정

//         canvasInstance = Instantiate(canvasPrefab); // 캔버스 생성
//         spectatorText = canvasInstance.GetComponentInChildren<SpectatorText>(); // 관전 대상 텍스트
//         spectatorText.SetSpectatingTarget(spectatingTarget); // UI에 관전 대상 표시

//         Debug.Log("Spectator Camera Start");
//     }

//     // Update로 관전 대상 변경 감지, 버츄얼 카메라 위치 업데이트
//     void Update()
//     {
//         // Handle switching between players
//         if (prevPlayerAction.triggered)
//         {
//             SwitchPlayer(-1);
//         }
//         if (nextPlayerAction.triggered)
//         {
//             SwitchPlayer(1);
//         }

//         UpdateVirtualCameraPosition();
//     }

//     // 버츄얼 카메라 위치 업데이트
//     private void UpdateVirtualCameraPosition()
//     {
//         this.transform.position = spectatingTarget.transform.position;
//         Vector2 mouseDelta = lookAction.ReadValue<Vector2>();

//         float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
//         float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

//         xRotation -= mouseY;
//         xRotation = Mathf.Clamp(xRotation, -90f, 90f);

//         transform.Rotate(Vector3.up * mouseX); // 카메라 좌우 회전
//     }

//     // 관전 대상 변경
//     private void SwitchPlayer(int idx_change)
//     {
//         if (remainingPlayers.Count == 0)
//             return;

//         currentPlayerIndex += idx_change;
//         if (currentPlayerIndex < 0)
//             currentPlayerIndex = remainingPlayers.Count - 1;
//         else if (currentPlayerIndex >= remainingPlayers.Count)
//             currentPlayerIndex = 0;

//         spectatingTarget = remainingPlayers[currentPlayerIndex];
//         spectatorText.SetSpectatingTarget(spectatingTarget); // UI에 관전 대상 표시
//         Debug.Log("Switching to player " + spectatingTarget.name);
//     }

//     // 다른 플레이어가 죽었을 때 SpectatorManager가 호출
//     public void RemovePlayer(GameObject player)
//     {
//         remainingPlayers.Remove(player);
//         if (spectatingTarget == player)
//         {
//             currentPlayerIndex = currentPlayerIndex % remainingPlayers.Count;
//             spectatingTarget = remainingPlayers[currentPlayerIndex];
//         }
//     }
// }
