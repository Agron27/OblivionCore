using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SCR_Movement : MonoBehaviour
{

    public float defaultSpeed = 2.5f;
    public float boostedSpeed = 3f;
    private float currentSpeed;
    public float jumpHeight = 1.3f;

    private float boostTimer = 0f;
    private float boostDuration = 3.5f;
    private float rechargeBoostTimer = 0f;
    private const float rechargeBoostDuration = 5f;


    public Camera playerCamera;

    private CharacterController characterController;

    private PlayerControls playerControls;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction boostAction;
    private InputAction switchGravity;
    //private InputAction lookAction;


    private AudioSource audioSource;
    public AudioClip footStepSound;

    private float footStepDelay = 0.5f;
    private float nextFootstep = 0f;


    // Gravity handling 
    public float gravity = -2f;
    private float groundDistance = 0.2f;
    public Transform groundChecker;
    public LayerMask groundMask;
    private bool isGrounded;
    private Vector3 velocity;


    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        moveAction = playerControls.Player.Move;
        jumpAction = playerControls.Player.Jump;
        boostAction = playerControls.Player.Boost;
        switchGravity = playerControls.Player.SwitchGravity;
        //lookAction = playerControls.Player.Look;

        moveAction.Enable();
        jumpAction.Enable();
        boostAction.Enable();
        switchGravity.Enable();
        //lookAction.Enable();


        switchGravity.performed += SwitchGravity;
    }



    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        boostAction.Disable();
        switchGravity.Disable();
        //lookAction.Disable();

        switchGravity.performed -= SwitchGravity;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask, QueryTriggerInteraction.Ignore);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        ApplyGravity();
        ApplyMovement(CalculateMovementInput());

        Boost();
        Jump();

        HandleFootSteps();
    }

    private void HandleFootSteps()
    {
        if (moveAction.triggered && isGrounded)
        {
            nextFootstep -= Time.deltaTime;
            if (nextFootstep <= 0)
            {
                audioSource.PlayOneShot(footStepSound, 0.7f);
                nextFootstep += footStepDelay;
            }
        }
    }



    Vector3 CalculateMovementInput()
    {
        Vector2 movementInput = moveAction.ReadValue<Vector2>();
        //Vector2 lookInput = lookAction.ReadValue<Vector2>();

        float moveHorizontal = movementInput.x;
        float moveVertical = movementInput.y;

        Vector3 forwardMovement = playerCamera.transform.forward * moveVertical;
        Vector3 rightMovement = playerCamera.transform.right * moveHorizontal;
        forwardMovement.y = 0;
        rightMovement.y = 0;

        return (forwardMovement + rightMovement).normalized * currentSpeed;
    }

    private void Boost()
    {
        if (CanBoost())
        {
            ApplyBoost();
        }
        else
        {
            ResetToDefaultSpeed();

            if (!boostAction.IsPressed())
            {
                boostTimer = 0f;
            }

            StartRechargeTimerIfBoostDepleted();
        }

        UpdateRechargeTimer();
    }

    private void Jump()
    {
        if (jumpAction.triggered && isGrounded)
        {
            // Calculate jump velocity
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    private void ApplyMovement(Vector3 movementInput)
    {
        Vector3 totalMovement = (movementInput * Time.deltaTime) + (velocity * Time.deltaTime);
        characterController.Move(totalMovement);
    }

    private bool CanBoost()
    {
        return boostAction.IsPressed() && boostTimer < boostDuration && rechargeBoostTimer <= 0f;
    }

    private void ApplyBoost()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, boostedSpeed, 0.2f * Time.deltaTime);
        boostTimer += Time.deltaTime;
        // play boost sound
    }

    private void ResetToDefaultSpeed()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, defaultSpeed, 1f * Time.deltaTime);
    }

    private void StartRechargeTimerIfBoostDepleted()
    {
        if (boostTimer >= boostDuration)
        {
            rechargeBoostTimer = rechargeBoostDuration;
        }
    }

    private void UpdateRechargeTimer()
    {
        if (rechargeBoostTimer > 0f)
        {
            rechargeBoostTimer -= Time.deltaTime;
        }
    }

    private void SwitchGravity(InputAction.CallbackContext context)
    {
        if (gravity == -2f)
        {
            gravity = -9.81f;
        }
        else
        {
            gravity = -2f;
        }
    }
}
