using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateManager : MonoBehaviour
{
    private CharacterMovement movement;
    private Rigidbody rb;
        
    [SerializeField] public bool isJumping = false;
    [SerializeField] public bool isGrounded = false;
    [SerializeField] public bool isRunning = false;
    [SerializeField] public bool isWalking = false;
    [SerializeField] public bool isWalkingUp = false;
    [SerializeField] public bool isCharging = false;


    [SerializeField] public int chargeCooldown = 0;

    void Awake()
    {
        movement = GetComponent<CharacterMovement>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandleCooldowns();
        HandleChargeAttack();

        isCharging = IsCharging();
        isRunning = IsRunning();
        isWalking = IsWalking();
        isWalkingUp = IsWalkingUp();
    }

    private bool IsWalking()
    {
        return rb && (rb.velocity.x != 0 || rb.velocity.z != 0) && !isJumping && !isRunning && !isCharging;
    }

    public bool IsWalkingUp()
    {
        return rb && rb.velocity.z > 0 && isWalking;
    }

    public bool IsRunning()
    {
        return isRunning && !isCharging;
    }

    public bool IsCharging()
    {
        return isCharging && !isGrounded;
    }

    public bool CanWalk()
    {
        return !isJumping && isGrounded && !isRunning && !isCharging;
    }

    public bool CanJump()
    {
        return !isJumping && isGrounded && !isCharging;
    }

    public bool CanRun()
    {
        return !isJumping && isGrounded;
    }

    public bool CanCharge()
    {
        return !isJumping && isGrounded && isRunning && chargeCooldown == 0;
    }

    public bool CanMove()
    {
        return !isCharging;
    }

    public void PerformChargeAttack()
    {
        isGrounded = false;
        isRunning = false;
        isCharging = true;
    }

    private void HandleCooldowns()
    {
        if (chargeCooldown > 0)
        {
            chargeCooldown--;
        }
    }

    private void HandleChargeAttack()
    {
        if(isCharging)
        {
            chargeCooldown = 20;
        }
    }
}