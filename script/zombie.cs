using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float wanderRadius = 10f;

    private Vector3 target;
    private Animator animator;
    private Rigidbody[] ragdollBodies;
    
    // Add this to stop the zombie from trying to walk after it dies
    private bool isDead = false; 

    void Start()
    {
        animator = GetComponent<Animator>();
        ragdollBodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = true;
        }

        PickNewTarget();
    }

    void Update()
    {
        // If the zombie is dead, stop running the movement code completely
        if (isDead) return; 

        Vector3 direction = target - transform.position;
        direction.y = 0;

        if (direction.magnitude < 1f)
        {
            PickNewTarget();
        }

        transform.position += direction.normalized * moveSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void PickNewTarget()
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(-wanderRadius, wanderRadius),
            0,
            Random.Range(-wanderRadius, wanderRadius)
        );

        target = randomPoint;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Prevent multiple hits triggering the code over and over
        if (isDead) return; 

        if (collision.gameObject.CompareTag("Player"))
        {
            isDead = true; // Mark as dead so Update() stops moving it

            if (animator != null)
            {
                animator.enabled = false;
            }

            foreach (Rigidbody rb in ragdollBodies)
            {
                rb.isKinematic = false;
            }

            Rigidbody carRb = collision.gameObject.GetComponent<Rigidbody>();

            if (carRb != null)
            {
                foreach (Rigidbody rb in ragdollBodies)
                {
                    // Apply the force so it feels weighty
                    rb.AddForce(carRb.linearVelocity * 2f, ForceMode.Impulse);
                }
            }

            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.ZombieHit();
            }

            // REMOVED THE DESTROY LOGIC HERE to comply with PDF instructions.
            // The zombie will now just lay there as a ragdoll forever.
        }
    }
}