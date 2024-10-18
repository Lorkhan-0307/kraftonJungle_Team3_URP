using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f; // ���콺 ����
    public Transform cameraPivot; // ī�޶� �Ǻ� ������Ʈ (���� �̵��� ���� �Ǻ�)
    public Transform cameraTransform; // ī�޶� �Ǻ� ������Ʈ (���� �̵��� ���� �Ǻ�)
    public float maxVerticalAngle = 80f; // ī�޶� ���� ���� ����

    private Vector3 latestPos; // ��Ʈ��ũ���� ����ȭ�� ��ġ
    private Quaternion latestRot; // ��Ʈ��ũ���� ����ȭ�� ȸ��
    private float verticalRotation = 0f; // ī�޶��� ���� ����
    private float horizontalRotation = 0f; // �÷��̾��� �¿� ����

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
            // �ٸ� �÷��̾�� ����ȭ�� ��ġ�� �̵�
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
    // �÷��̾� �̵� ó�� (WASD)
    void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection); // �÷��̾��� ���� ���⿡ ���� �̵�
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    // ���콺 �Է¿� ���� ���� ����
    void HandleMouseLook()
    {
        // ���콺 �¿� �����ӿ� ���� �÷��̾� ȸ��
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        horizontalRotation += mouseX;
        transform.localRotation = Quaternion.Euler(0, horizontalRotation, 0);

        // ���콺 ���� �����ӿ� ���� ī�޶� �Ǻ��� ȸ�� (Pitch)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);
        cameraPivot.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    // ����ĳ��Ʈ ó��
    void HandleRaycast()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward); // ī�޶��� �������� ����ĳ��Ʈ
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f) && hit.rigidbody) // 100���� ���� ������ ����ĳ��Ʈ
        {
            if (hit.rigidbody.CompareTag("Entity"))
            {
                object content = $"{GetComponent<PhotonView>().ViewID.ToString()},{hit.rigidbody.GetComponent<PhotonView>().ViewID.ToString()}"; // �̺�Ʈ�� ������ ������ (�ʿ��)
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }; // ��� Ŭ���̾�Ʈ���� ����
                SendOptions sendOptions = new SendOptions { Reliability = true }; // �ŷڼ� ����

                PhotonNetwork.RaiseEvent((byte)EventCode.AttackToServer, content, raiseEventOptions, sendOptions);
            }
            else
            {
                Debug.Log("����ĳ��Ʈ�� ���� ������Ʈ: " + hit.rigidbody.name);
            }
        }
        else
        {
            Debug.Log("����ĳ��Ʈ�� �ƹ��͵� ������ �ʾҽ��ϴ�.");
        }
    }

    // ��Ʈ��ũ ����ȭ�� ���� �޼���
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // ���� �����͸� ������ ��
        {
            stream.SendNext(transform.position); // ��ġ ����
            stream.SendNext(transform.rotation); // ȸ�� ����
        }
        else // �ٸ� �÷��̾��� �����͸� ���� ��
        {
            latestPos = (Vector3)stream.ReceiveNext(); // ��ġ �ޱ�
            latestRot = (Quaternion)stream.ReceiveNext(); // ȸ�� �ޱ�
        }
    }
}
