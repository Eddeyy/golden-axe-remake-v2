using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStateManager))]
public class ChargeAttack : MonoBehaviour
{
    private CharacterStateManager state;
    private Rigidbody rb;
    void Awake()
    {
        state = GetComponent<CharacterStateManager>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(state.isGrounded)
        {
            state.isCharging = false;
        }
    }

    public void StartChargeAttack()
    {
        if(state.CanCharge())
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);

            state.PerformChargeAttack();

            float xforce = 9f;
            xforce = (transform.rotation.eulerAngles.y == 0)? xforce : -xforce;

            rb.AddForce(new Vector3(xforce, 2f,0f), ForceMode.Impulse);
        }
    }
}
