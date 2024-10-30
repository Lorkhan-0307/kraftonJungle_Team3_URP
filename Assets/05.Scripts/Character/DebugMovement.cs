// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;
// using UnityEngine.InputSystem;
// using UnityEngine.InputSystem.HID;
// using UnityEngine.UI;

// public class DebugMovement : MonoBehaviour
// {
//     public CharacterController controller;
//     public float speed = 4f;
//     public float monsterNightSpeed = 8f;
//     public float gravity = -9.81f;
//     public float jumpHeight = 3f;
//     public Transform groundCheck;
//     public float groundDistance = 0.4f;
//     public LayerMask groundMask;
//     Vector3 velocity;
//     bool isGrounded;

//     public AudioClip footStepSound;
//     public float footStepDelay;

//     private float nextFootstep = 0;

//     // Input Actions 변수
//     private PlayerInput playerInput;
//     private InputAction moveAction;
//     private InputAction jumpAction;
//     private InputAction killAction;
//     private InputAction runAction;
//     private InputAction interactAction;

//     [SerializeField] private Transform raycastShootPos;
//     [SerializeField] private float attackrange = 3f;
//     private Button killButton;
//     private Button interactButton;

//     public Player player;

//     //Using Raycast
//     RaycastHit hit;
//     GameObject target;
//     GameObject interactTarget;

//     // 쿨타임 시간
//     public float killCooltime = 5f;
//     // 현재 남은 쿨타임
//     private float currentCooltime = 0f;
//     private bool isKillOn = false;

//     // UI 쿨타임 이미지
//     private Image killButtonImage;

//     public Animator animator;
//     public Animator fpsAnimator;

//     [SerializeField] private GameObject scientistFPS;
//     [SerializeField] private GameObject monsterFPS;

//     private void Awake()
//     {
//         // PlayerInput 컴포넌트에서 InputAction 가져오기
//         playerInput = GetComponent<PlayerInput>();
//         moveAction = playerInput.actions["Move"];
//         jumpAction = playerInput.actions["Jump"];
//         killAction = playerInput.actions["Kill"];
//         runAction = playerInput.actions["Run"];
//         interactAction = playerInput.actions["Interact"];

//         controller = GetComponentInParent<CharacterController>();

//         player = GetComponentInParent<Player>();

//         // 부모 오브젝트
//         Transform parentTransform = transform.parent;

//         fpsAnimator = GetComponentInChildren<Animator>();
//     }

//     private void Start()
//     {
//         killButtonImage = FindObjectOfType<KillButton>().GetComponent<Image>();
//         // 시작할 때 쿨타임 초기화
//         currentCooltime = 0f;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         isGrounded = controller.isGrounded;
//         // animator.SetBool("IsGrounded", isGrounded);

//         if (isGrounded && velocity.y < 0)
//         {
//             velocity.y = -2f;
//         }

//         // Move 액션으로 이동 입력 받기
//         Vector2 input = moveAction.ReadValue<Vector2>();
//         Vector3 motion = transform.right * input.x + transform.forward * input.y;
//         controller.Move(motion * Time.deltaTime);

//         // Jump 액션으로 점프 입력 받기
//         if (jumpAction.triggered && isGrounded)
//         {
//             velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
//             animator.SetTrigger("Jump");
//             Debug.Log("JUMP ACTION");
//         }

//         // 중력 적용
//         velocity.y += gravity * Time.deltaTime;
//         controller.Move(velocity * Time.deltaTime);



//         // 발걸음 소리 재생
//         if ((input.x != 0 || input.y != 0) && isGrounded)
//         {
//             nextFootstep -= Time.deltaTime;
//             if (nextFootstep <= 0)
//             {
//                 GetComponent<AudioSource>().PlayOneShot(footStepSound, 0.7f);
//                 nextFootstep += footStepDelay;
//                 //Debug.Log("Walking true");
//                 // bool 파라미터 설정
//                 // animator.SetBool("IsWalking", true);
//                 // fpsAnimator.SetBool("IsWalking", true);
//             }
//         }
//         else
//         {
//             //Debug.Log("Walking false");
//             // animator.SetBool("IsWalking", false);
//             // fpsAnimator.SetBool("IsWalking", false);
//         }


//         // 점프 : is grounded
//         if ()

//         // 애니메이션 상태 변경
//         animator.SetFloat("Speed", input.magnitude);
//         animator.SetBool("isRunning", runAction.IsPressed());
//         Debug.Log("Speed: " + input.magnitude);
//     }
// }