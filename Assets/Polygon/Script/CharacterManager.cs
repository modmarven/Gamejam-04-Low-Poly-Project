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
    [SerializeField] private float currentSpeed;
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float walkBackSpeed = 2.0f;

    [Space]
    public float turnSmoothVelocity;
    public float turnSmoothTime = 1.0f;
    private Vector3 velocity;
    [Space]
    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private float runBackSpeed = 4.0f;

    public bool isWalk;
    public bool isSprint;


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
        currentSpeed = walkSpeed;

        // Cursor visible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    void Update()
    {
        HandlingCharacterMovement();
        HandlingCharacterAnimation();
        CharacterApplyGravity();
        HandlingCharacterSprint();

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

    private void HandlingCharacterSprint()
    {
        // get movement input
        Vector2 inputVector = inputSystem.Player.Move.ReadValue<Vector2>();

        // Set sprint bool
        if (inputSystem.Player.Sprint.triggered) isSprint = !isSprint;
        if (!isSprint) isWalk = true;
        else isWalk = false;

        if (inputVector == Vector2.zero) isSprint = false;

        // Handle the speed
        if (isSprint && inputVector.y > 0.0f) currentSpeed = runSpeed;
        else if (isSprint && inputVector.y < 0.0f) currentSpeed = runBackSpeed;
        if (isWalk && inputVector.y > 0.0f) currentSpeed = walkSpeed;
        else if (isWalk && inputVector.y < 0.0f) currentSpeed = walkBackSpeed;
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
        if (isSprint) animator.SetBool("isRun", true);
        else animator.SetBool("isRun", false);

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
