using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController _characterController;
    private Vector2 _moveContext;
    private float _verticalVelocity;
    private bool _isGrounded;

    public Transform cameraTransform;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 4.5f;
    public float initialJumpGravity = -30f;
    public float jumpGravity = -8f;
    public float fallGravity = -50f;


    public void MoveInputAction(InputAction.CallbackContext context)
    {
        _moveContext = context.ReadValue<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {

        HandleMovement();

    }

    private void HandleMovement()
    {
        _isGrounded = _characterController.isGrounded;

        if (_isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2f; // 약간의 음수 값을 주어 땅에 붙어있도록 설정
        }

        // 애니메이션 상태 확인
        //AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        //bool isLanding = stateInfo.IsName("flower|Down"); // "Down" 상태인지 확인 (착지 애니메이션)

        Vector3 moveDirection = Vector3.zero;

        //_animator.SetFloat(Magnitude, _moveContext.magnitude);

        if (_moveContext.sqrMagnitude > 0.01f)
        {
            // 카메라의 방향을 기준으로 캐릭터 이동 방향 설정
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0f;  // 수직 방향 제거하여 평면 상에서 이동
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            moveDirection = forward * _moveContext.y + right * _moveContext.x;
            moveDirection.Normalize();

            // 캐릭터 회전 (카메라 방향을 따라)
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 수직 속도 업데이트 (중력 적용)
        if (_verticalVelocity > 0)
        {
            // 올라가는 동안 중력 적용
            if (_verticalVelocity == jumpForce)
            {
                _verticalVelocity += initialJumpGravity * Time.deltaTime; // 초기 큰 중력
            }
            else
            {
                _verticalVelocity += jumpGravity * Time.deltaTime; // 이후 감소하는 중력
            }
        }
        else
        {
            // 내려가는 동안 중력 적용
            _verticalVelocity += fallGravity * Time.deltaTime;
            //_animator.SetBool(IsDown, true); // 땅에 착지하면 is_down을 true로 설정
        }

        // 캐릭터 이동 (수평 + 수직)
        moveDirection *= moveSpeed;
        moveDirection.y = _verticalVelocity;
        _characterController.Move(moveDirection * Time.deltaTime);
    }


}
