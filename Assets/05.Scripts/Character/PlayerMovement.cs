using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 8f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;

    public AudioClip footStepSound;
    public float footStepDelay;

    private float nextFootstep = 0;

    // Input Actions 변수
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction killAction;

    [SerializeField] private Transform raycastShootPos;
    [SerializeField] private float attackrange = 3f;
    [SerializeField] private Button killButton;
    
    
    //Using Raycast
    RaycastHit hit;
    GameObject target;
    

    private void Awake()
    {
        // PlayerInput 컴포넌트에서 InputAction 가져오기
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        killAction = playerInput.actions["Kill"];
    }

    // Update is called once per frame
    void Update()
    {
        // 바닥 체크
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        
        
        isGrounded = controller.isGrounded;
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Move 액션으로 이동 입력 받기
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 motion = transform.right * input.x + transform.forward * input.y;
        controller.Move(motion * speed * Time.deltaTime);

        // Jump 액션으로 점프 입력 받기
        if (jumpAction.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log("JUMP ACTION");
        }

        // 중력 적용
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


        // Using RayCast to detect attack
        RayCastAttackDetection();
        
        
        
        
        if (killButton.interactable && killAction.triggered && target != null)
        {
            AttackAction();   
        }

        // 발걸음 소리 재생
        if ((input.x != 0 || input.y != 0) && isGrounded)
        {
            nextFootstep -= Time.deltaTime;
            if (nextFootstep <= 0)
            {
                GetComponent<AudioSource>().PlayOneShot(footStepSound, 0.7f);
                nextFootstep += footStepDelay;
            }
        }
    }


    private void RayCastAttackDetection()
    {
        if (Physics.Raycast(raycastShootPos.position, transform.forward, out hit, attackrange))
        {
            if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("NPC"))
            {
                //Debug.Log("DETECT PLAYER");
                killButton.interactable = true;
                target = hit.collider.gameObject;
            }
            else
            {
                killButton.interactable = false;
                target = null;
            }
        }
        else
        {
            killButton.interactable = false;
            target = null;
        }
    }

    private void AttackAction()
    {
        Player player = GetComponent<Player>();
        player.OnAttack(target);
        Player targetPlayer = target.GetComponent<Player>();
        targetPlayer.OnDamaged(this.gameObject);
    }
}
