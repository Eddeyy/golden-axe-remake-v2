using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(CharacterStateManager))]
[RequireComponent(typeof(CharacterMovement))]
public class PlayerCharacterController : MonoBehaviour
{
    const float DOUBLE_TAP_TIME = 0.4f;

    public CharacterStateManager state;
    private CharacterMovement characterMovement;
    private ChargeAttack chargeAttack;
    private PlayerControls playerControls; 
    [SerializeField] float lastTap;
    [SerializeField] float currentTap;
    [SerializeField] private Vector3 moveDirection = Vector3.zero;

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>(); // Pobieranie komponentu Movement
        state = GetComponent<CharacterStateManager>();
        chargeAttack = GetComponent<ChargeAttack>();

        playerControls = new PlayerControls();
        playerControls.InGame.Enable();
        playerControls.InGame.Jump.performed += Jump;
    }
    void Update()
    {
        GetDoubleTaps();
    }

    private void FixedUpdate()
    {
        characterMovement.MoveCharacter(moveDirection);
    }

    private Vector3 GetPlayerInput()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        return new Vector3(moveHorizontal, 0f , moveVertical);
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        moveDirection = new Vector3(value.x, 0f, value.y);
    }

    void Jump(InputAction.CallbackContext context)
    {
        if(context.performed)
            characterMovement.Jump();
    }

    void GetDoubleTaps() 
    {

        if (Input.GetButtonDown("Horizontal") && state.CanRun())
        {
            currentTap = Time.time;
            if (Input.GetAxis("Horizontal") != 0)
            {
                if (currentTap - lastTap <= DOUBLE_TAP_TIME && state.isGrounded)
                    state.isRunning = true;
                lastTap = currentTap;
            }
        }

        if (!Input.GetButton("Horizontal") && state.isGrounded)
        {
            state.isRunning = false;
        }

    }

    void GetAttackButton()
    {
        if(Input.GetButtonDown("Attack"))
        {
            chargeAttack.StartChargeAttack();
        }
        
    }
}
