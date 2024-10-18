using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f; // 마우스 감도
    public Transform cameraPivot; // 카메라 피봇 오브젝트 (상하 이동을 위한 피봇)
    public Transform cameraTransform; // 카메라 피봇 오브젝트 (상하 이동을 위한 피봇)
    public float maxVerticalAngle = 80f; // 카메라 상하 제한 각도

    private Vector3 latestPos; // 네트워크에서 동기화된 위치
    private Quaternion latestRot; // 네트워크에서 동기화된 회전
    private float verticalRotation = 0f; // 카메라의 상하 각도
    private float horizontalRotation = 0f; // 플레이어의 좌우 각도

    private void Awake()
    {
        if(photonView.IsMine)
        {
            AddCamera();
        }
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            HandleMovement();
            HandleMouseLook();

            if (Input.GetMouseButtonDown(0))
            {
                HandleRaycast();
            }
        }
        else
        {
            // 다른 플레이어는 동기화된 위치로 이동
            transform.position = Vector3.Lerp(transform.position, latestPos, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, latestRot, Time.deltaTime * 10);
        }
    }

    [SerializeField]
    Transform camPos;
    void AddCamera()
    {
        Destroy(Camera.main.gameObject);

        GameObject newG = new GameObject("MainCamera");
        newG.transform.parent = camPos;
        newG.transform.localPosition = Vector3.zero;
        newG.transform.localScale = Vector3.one;
        newG.transform.rotation = Quaternion.identity;
        newG.AddComponent<Camera>();
        newG.tag = "MainCamera";
        cameraTransform = newG.transform;
    }
    // 플레이어 이동 처리 (WASD)
    void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection); // 플레이어의 보는 방향에 따라 이동
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    // 마우스 입력에 따른 시점 조절
    void HandleMouseLook()
    {
        // 마우스 좌우 움직임에 따라 플레이어 회전
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        horizontalRotation += mouseX;
        transform.localRotation = Quaternion.Euler(0, horizontalRotation, 0);

        // 마우스 상하 움직임에 따라 카메라 피봇의 회전 (Pitch)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);
        cameraPivot.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    // 레이캐스트 처리
    void HandleRaycast()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward); // 카메라의 전방으로 레이캐스트
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f) && hit.rigidbody) // 100미터 범위 내에서 레이캐스트
        {
            if (hit.rigidbody.CompareTag("Entity"))
            {
                object content = $"{GetComponent<PhotonView>().ViewID.ToString()},{hit.rigidbody.GetComponent<PhotonView>().ViewID.ToString()}"; // 이벤트에 포함할 데이터 (필요시)
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }; // 모든 클라이언트에게 전송
                SendOptions sendOptions = new SendOptions { Reliability = true }; // 신뢰성 보장

                PhotonNetwork.RaiseEvent((byte)EventCode.AttackToServer, content, raiseEventOptions, sendOptions);
            }
            else
            {
                Debug.Log("레이캐스트에 맞은 오브젝트: " + hit.rigidbody.name);
            }
        }
        else
        {
            Debug.Log("레이캐스트가 아무것도 맞추지 않았습니다.");
        }
    }

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
            latestPos = (Vector3)stream.ReceiveNext(); // 위치 받기
            latestRot = (Quaternion)stream.ReceiveNext(); // 회전 받기
        }
    }
}
