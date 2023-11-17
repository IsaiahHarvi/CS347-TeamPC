using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneEnemy : MonoBehaviour
{
    // Movement
    private Rigidbody rb;
    public float flyingSpeed = 5f;
    public float hoverHeight = 10f;

    // Target Tracking
    public Transform target;

    // Attack
    public GameObject projectilePrefab;
    public float attackRate = 2f; // Time in seconds between attacks
    private float lastAttackTime = 0f;

    void Start()
    {
        // Initialize Rigidbody
        rb = GetComponent<Rigidbody>();

        // Set target to a GameObject with tag 'Player' if target is null
        if (target == null)
        {
            GameObject targetObject = GameObject.FindGameObjectWithTag("Player");
            if (targetObject != null)
            {
                target = targetObject.transform;
            }
            else
            {
                Debug.LogError("No GameObject with tag 'Player' found in the scene.");
            }
        }
    }

    void Update()
    {
        // Target Tracking and Movement
        if (target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y + hoverHeight, target.position.z);
            Vector3 direction = targetPosition - transform.position;
            rb.velocity = direction.normalized * flyingSpeed;

            // Attack logic
            if (Time.time - lastAttackTime > attackRate)
            {
                ShootAtTarget();
                lastAttackTime = Time.time;
            }
        }
        else
        {
            Debug.LogWarning("Target is null.");
        }
    }

    void ShootAtTarget()
    {
        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        
        // Calculate the direction to shoot the projectile
        Vector3 shootDirection = (target.position - transform.position).normalized;
        projectile.GetComponent<Rigidbody>().velocity = shootDirection * 10f; // Adjust speed as needed
    }
}
