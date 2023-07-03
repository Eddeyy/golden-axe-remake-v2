using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateManager : MonoBehaviour
{
    private CharacterMovement movement;
    private Rigidbody rb;
    
    private Animator animator;
        
    [SerializeField] public bool isJumping = false;
    [SerializeField] public bool isFalling = false;
    [SerializeField] public bool isGrounded = false;
    [SerializeField] public bool isRunning = false;
    [SerializeField] public bool isWalking = false;
    [SerializeField] public bool isWalkingUp = false;
    [SerializeField] public bool isCharging = false;


    [SerializeField] public int chargeCooldown = 0;
    [SerializeField] public int walkAnimCooldown = 0;
    
    [SerializeField] public bool isIdle = true;

    void Awake()
    {
        movement = GetComponent<CharacterMovement>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandleCooldowns();
        HandleChargeAttack();
        ManageWalkCooldown();

        isCharging = IsCharging();
        isRunning = IsRunning();
        isWalking = IsWalking();
        isWalkingUp = IsWalkingUp();
        isFalling = IsFalling();
        isIdle = IsIdle();

        AssignAnimatorParams();
    }

    private void AssignAnimatorParams()
    {
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isWalkingUp", isWalkingUp);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isCharging", isCharging);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isFalling", isFalling);
        animator.SetInteger("walkAnimCooldown", walkAnimCooldown);
    }

    private void ManageWalkCooldown()
    {
        if(Mathf.Abs(rb.velocity.x) > 0.1f || Mathf.Abs(rb.velocity.z) > 0.1f)
        {
            animator.speed = 1f;
            walkAnimCooldown = 40;
            return;
        }

        if(walkAnimCooldown > 0)
        {
            animator.speed = 0f;
            walkAnimCooldown--;
        }
        else
        {
            animator.speed = 1f;
        }
    }

    private bool IsWalking()
    {
        return rb && (rb.velocity.x != 0 || rb.velocity.z != 0) && !isJumping && !isRunning && !isCharging && isGrounded;
    }

    public bool IsWalkingUp()
    {
        return rb && rb.velocity.z > 0 && isWalking;
    }

    public bool IsRunning()
    {
        return isRunning;
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

    public bool IsFalling()
    {
        return rb.velocity.y < 0 && !isGrounded && !isCharging;
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
        isCharging = true;
    }

    private void HandleCooldowns()
    {
        if (chargeCooldown == 20 && !isCharging)
        {
            isRunning = false;
        }

        if (chargeCooldown > 0 && !isCharging)
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

    private bool IsIdle()
    {
        return !isWalking && !isWalkingUp && !isCharging && !isRunning && !isJumping && !isFalling && isGrounded;
    }
}
