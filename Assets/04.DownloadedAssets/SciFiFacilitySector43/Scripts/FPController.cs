using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPController : MonoBehaviour
{
    public float speed = 6f;
    public float mouseSensitivity = 5f;
    public float jumpSpeed = 10f;

    private float rotationLeftRight;
    private float verticalRotation;
    private float forwardSpeed;
    private float sideSpeed;
    private float verticalVelocity;
    private Vector3 speedCombined;
    private CharacterController cc;
    private Camera cam;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isJumping;
    private bool isRunning;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        cc = GetComponent<CharacterController>();
        Cursor.visible = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isJumping = true;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    void Update()
    {
        // Mouse Look
        rotationLeftRight = lookInput.x * mouseSensitivity;
        transform.Rotate(0, rotationLeftRight, 0);

        verticalRotation -= lookInput.y * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -60f, 60f);
        cam.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        // Movement Input
        forwardSpeed = moveInput.y * speed;
        sideSpeed = moveInput.x * speed;

        if (isRunning)
        {
            forwardSpeed *= 2f;
        }

        // Gravity
        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        // Jump
        if (cc.isGrounded && isJumping)
        {
            verticalVelocity = jumpSpeed;
            isJumping = false; // Reset jump after use
        }

        // Move Character
        speedCombined = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);
        speedCombined = transform.rotation * speedCombined;
        cc.Move(speedCombined * Time.deltaTime);
    }
}
