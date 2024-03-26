using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterManager : MonoBehaviour
{
    private Movement inputSystem;
    private CharacterController characterController;
    private Animator animator;

    [Header("Movement Setting")]
    public float currentSpeed = 3.0f;
    public float walkSpeed = 3.0f;
    public float turnSmoothVelocity;
    public float turnSmoothTime = 1.0f;
    private Vector3 velocity;

    [Header("Jump Setting")]
    public float gravity = 9.81f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundDistance = 0.4f;
    public bool isGrounded;

    
    void Awake()
    {
        inputSystem = new Movement();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Cursor visible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    void Update()
    {
        HandlingCharacterMovement();
        HandlingCharacterAnimation();
        CharacterApplyGravity();

        //Check if grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
    }

    private void HandlingCharacterMovement()
    {
        Vector2 inputVector = inputSystem.Player.Move.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y).normalized;
        moveDirection = transform.TransformDirection(moveDirection);

        characterController.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);
        
    }

    private void CharacterApplyGravity()
    {
        if (!isGrounded) velocity.y -= gravity * Time.deltaTime;
        else if (velocity.y < 0) velocity.y = -2f;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandlingCharacterAnimation()
    {
        Vector2 inputVector = inputSystem.Player.Move.ReadValue<Vector2>();
        animator.SetFloat("xMove", inputVector.x);
        animator.SetFloat("yMove", inputVector.y);
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Enable();
    }
}
