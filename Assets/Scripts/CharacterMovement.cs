using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStateManager))]
public class CharacterMovement : MonoBehaviour
{
    public CharacterStateManager state;

    [SerializeField]
    Collider collider;

    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    float speed = 5f;

    LayerMask groundLayer;
    private float jumpForce = 10f;

    private Vector3 airVelocity = Vector3.zero;
    private const float MAX_AIR_VELOCITY = 5f; 

    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        state = GetComponent<CharacterStateManager>();
        collider = GetComponent<Collider>();
        groundLayer = LayerMask.NameToLayer("Ground");
    }

    void FixedUpdate()
    {
        state.isGrounded = IsGrounded();
        if(state.isJumping && state.isGrounded)
            Debug.LogWarning("Coś się zesrało");
    }
    
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == groundLayer && state.isJumping)
        {
            state.isJumping = false;
            airVelocity = Vector3.zero;
        }
    }

    public void MoveCharacter(Vector3 moveDirection)
    {
        moveDirection *= (state.isRunning)? speed * 2f : speed;

        if(!state.CanMove())
            return;
       
        FlipInMoveDirection(moveDirection);

        if(state.isJumping)
        {   
            moveDirection *= 0.005f;
            airVelocity = (state.isRunning)? Vector3.ClampMagnitude(airVelocity, MAX_AIR_VELOCITY*2f) : Vector3.ClampMagnitude(airVelocity, MAX_AIR_VELOCITY);
            rb.velocity = new Vector3(airVelocity.x + moveDirection.x, rb.velocity.y, airVelocity.z + moveDirection.z);
            airVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }
        else
        {
            rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        }
    }

    public void Jump()
    {
        jumpForce = (state.isRunning)? 9.5f : 8.5f; 

        if (state.CanJump())
        {
            rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
            state.isJumping = true;
            airVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }
    }

    private void FlipInMoveDirection(Vector3 moveDirection)
    {
        if (moveDirection.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Obrót w prawo (bez zmiany rotacji)
        }
        else if (moveDirection.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Obrót w lewo (odwrócenie w osi Y o 180 stopni)
        }
    }
    private bool IsGrounded()
    {
        float maxDistance = 0.3f;

            RaycastHit hit;

            var position = transform.position;

            int layerMask = ~(1 << LayerMask.NameToLayer("Detectors") | 1 << LayerMask.NameToLayer("Hitboxes"));

            if (Physics.Raycast(new Vector3(position.x - collider.bounds.extents.x,position.y - collider.bounds.extents.y,position.z), -Vector3.up, out hit, maxDistance, layerMask))
                {
                    Debug.Log(LayerMask.LayerToName(hit.transform.gameObject.layer));
                    if (hit.transform.gameObject.layer == groundLayer)
                    {
                        return true;
                    }
                }

            if (Physics.Raycast(new Vector3(position.x + collider.bounds.extents.x,position.y - collider.bounds.extents.y,position.z), -Vector3.up, out hit, maxDistance, layerMask))
                {
                    Debug.Log(LayerMask.LayerToName(hit.transform.gameObject.layer));
                    if (hit.transform.gameObject.layer == groundLayer)
                    {
                        return true;
                    }
                }

        return false;
    }
}
