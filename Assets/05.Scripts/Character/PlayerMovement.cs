using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 4f;
    public float monsterNightSpeed = 8f;
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
    private InputAction runAction;
    private InputAction interactAction;

    [SerializeField] private Transform raycastShootPos;
    [SerializeField] private float attackrange = 3f;
    private Button killButton;
    private Button interactButton;

    public Player player;

    //Using Raycast
    RaycastHit hit;
    GameObject target;
    GameObject interactTarget;

    // 쿨타임 시간
    public float killCooltime = 5f;
    // 현재 남은 쿨타임
    private float currentCooltime = 0f;
    private bool isKillOn = false;

    // UI 쿨타임 이미지
    private Image killButtonImage;

    public Animator animator;
    public Animator fpsAnimator;

    [SerializeField] private GameObject scientistFPS;
    //[SerializeField] 
    public GameObject monsterFPS;

    public bool isAttacking = false;

    private void Awake()
    {
        // PlayerInput 컴포넌트에서 InputAction 가져오기
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        killAction = playerInput.actions["Kill"];
        runAction = playerInput.actions["Run"];
        interactAction = playerInput.actions["Interact"];

        controller = GetComponentInParent<CharacterController>();

        player = GetComponentInParent<Player>();

        // 부모 오브젝트
        Transform parentTransform = transform.parent;

        if (parentTransform != null)
        {
            GameObject targetObject = parentTransform.Find("Ch11_nonPBR@Idle").gameObject;
            SetLayerRecursive(targetObject, 3);
            animator = targetObject.GetComponent<Animator>();
            if (NetworkManager.Instance.IsMonster())
            {
                targetObject = parentTransform.Find("Parasite L Starkie").gameObject;
                SetLayerRecursive(targetObject, 3);
            }
        }
        fpsAnimator = GetComponentInChildren<Animator>();
    }

    public void SetLayerRecursive(GameObject obj, int newLayer)
    {
        // 현재 오브젝트의 레이어 변경
        obj.layer = newLayer;

        // 모든 자식 오브젝트의 레이어 변경
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursive(child.gameObject, newLayer);
        }
    }

    private void Start()
    {
        if (killButton == null)
        {
            killButton = FindObjectOfType<KillButton>().GetComponent<Button>();
        }
        if (interactButton == null)
        {
            interactButton = GameObject.Find("Button_interact").GetComponent<Button>();
            interactButton.interactable = false;
        }
        killButtonImage = FindObjectOfType<KillButton>().GetComponent<Image>();
        // 시작할 때 쿨타임 초기화
        currentCooltime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (!isAttacking)
        {
            // Move 액션으로 이동 입력 받기
            Vector2 input = moveAction.ReadValue<Vector2>();
            Vector3 motion = transform.right * input.x + transform.forward * input.y;
            float currentSpeed = IsMonsterNightSpeed() && runAction.IsPressed() ? monsterNightSpeed : speed;
            controller.Move(motion * currentSpeed * Time.deltaTime);        

            // Jump 액션으로 점프 입력 받기
            if (jumpAction.triggered && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                Debug.Log("JUMP ACTION");
            }


            // 발걸음 소리 재생
            if ((input.x != 0 || input.y != 0) && isGrounded)
            {
                nextFootstep -= Time.deltaTime;
                if (nextFootstep <= 0)
                {
                    GetComponent<AudioSource>().PlayOneShot(footStepSound, 0.7f);
                    nextFootstep += footStepDelay;
                    //Debug.Log("Walking true");
                    // bool 파라미터 설정
                    animator.SetBool("IsWalking", true);
                    fpsAnimator.SetBool("IsWalking", true);
                }
            }
            else
            {
                //Debug.Log("Walking false");
                animator.SetBool("IsWalking", false);
                fpsAnimator.SetBool("IsWalking", false);
            }

            // Using RayCast to detect attack
            RayCastAttackDetection();

            if (killButton.interactable && killAction.triggered && target != null && !isKillOn)
            {
                AttackAction();
                StartKillCooldown();
            }

            if (interactButton.interactable && interactAction.triggered && interactTarget != null)
            {
                Debug.Log("Interact");
                interactTarget.GetComponentInParent<Interact>().BroadcastInteraction();
            }
        }

        //
        // 중력 적용
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        //

        // 쿨타임 업데이트
        UpdatekillCooltime();

    }

    private void UpdatekillCooltime()
    {
        if (isKillOn)
        {
            currentCooltime -= Time.deltaTime;

            // UI 쿨타임 표시
            if (killButtonImage != null)
            {
                killButtonImage.fillAmount = 1 - currentCooltime / killCooltime;
            }

            if (currentCooltime <= 0)
            {
                isKillOn = false;
                currentCooltime = 0f;
            }
        }
    }

    private void StartKillCooldown()
    {
        isKillOn = true;
        currentCooltime = killCooltime;
    }

    private void RayCastInteractDetection(GameObject detectedObject)
    {

        Interact interactComponent;
        
        if(detectedObject.TryGetComponent<Interact>(out interactComponent))
        {
            if (interactComponent.isInteractable)
            {
                interactTarget = detectedObject;
                interactButton.interactable = true;
            }
            else interactButton.interactable = false;
        }
        else
        {
            interactTarget = null;
            interactButton.interactable = false;
        }
    }

    private void RayCastAttackDetection()
    {
        target = null;
        interactTarget = null;
        killButton.interactable = false;
        interactButton.interactable = false;
        
        if (Physics.Raycast(raycastShootPos.position, transform.forward, out hit, attackrange))
        {
            // 낮, 연구원 : 현재 로직
            // 밤, 연구원 : Kill X (Detect 호출 X)

            // 밤, 몬스터 : 현재 로직
            // 낮, 몬스터 : NPC만 Kill

            // obsolete version
            //target = hit.collider.gameObject;

            GameObject detectedGameObject = hit.collider.gameObject;


            bool canAttack = player.AttackDetection(detectedGameObject);

            if (canAttack && !isKillOn)
            {
                target = detectedGameObject;
                killButton.interactable = true;
            }
            //killButton.interactable = canAttack && !isKillOn;

            //if (!canAttack)
                //target = null;
                
            RayCastInteractDetection(detectedGameObject);
        }
        else
        {
            killButton.interactable = false;
            target = null;
        }
    }

    private void AttackAction()
    {
        player.OnAttack(target);
        Player targetPlayer = target.GetComponent<Player>();
        // 트리거 설정
        animator.SetTrigger("Stab");
        fpsAnimator.SetTrigger("Stab");
    }

    private bool IsAvailableToAttack()
    {
        // 시간에 따라 공격이 가능할수도, 안가능할 수도 있다. 이를 여기에서 식별한다.
        // TimeSwitchSlider 등에서 현재 시간이 낮인지, 밤인지를 불러오는 과정이 필요.
        bool isAttackable = true;
        switch (player.type)
        {
            case CharacterType.Monster:
                break;
            case CharacterType.Scientist:
                if (!TimeManager.instance.isDay) isAttackable = false;
                break;
            default:
                Debug.LogError("TYPE NOT SET!!");
                break;
        }

        return isAttackable;
    }

    private bool IsMonsterNightSpeed()
    {
        return !TimeManager.instance.isDay && player.type == CharacterType.Monster;
    }

    public void OnMonsterFPS()
    {
        scientistFPS.SetActive(false);
        monsterFPS.SetActive(true);
        fpsAnimator = monsterFPS.GetComponent<Animator>();
    }

    public void OffMonsterFPS()
    {
        scientistFPS.SetActive(true);
        monsterFPS.SetActive(false);
        fpsAnimator = scientistFPS.GetComponent<Animator>();
    }
}
    