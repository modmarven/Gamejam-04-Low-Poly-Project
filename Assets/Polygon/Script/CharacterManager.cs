using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Mouse Look Setting")]
    public Transform cameraTransform;

    
    void Awake()
    {
        inputSystem = new Movement();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        HandlingCharacterMovement();
        HandlingCharacterAnimation();
    }

    private void HandlingCharacterMovement()
    {
        Vector2 inputVector = inputSystem.Player.Move.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            Vector3 direction = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
        }
        
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
