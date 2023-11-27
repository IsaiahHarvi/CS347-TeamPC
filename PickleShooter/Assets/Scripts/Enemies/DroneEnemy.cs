using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneEnemy : MonoBehaviour
{
    // Movement
    private Rigidbody rb;
    public float flyingSpeed = 5f;
    public float hoverHeight = 10f;
    public float minimumDistance = 25f; 

    // Target Tracking
    public Transform target;

    // Attack
    public GameObject projectilePrefab;
    public float attackRate = 2f; 
    private float lastAttackTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

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
        if (target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y + hoverHeight, target.position.z);
            Vector3 direction = targetPosition - transform.position;

            // Maintain minimum distance from the target
            if (direction.magnitude > minimumDistance)
            {
                rb.velocity = direction.normalized * flyingSpeed;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }

            Vector3 directionToTarget = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            Quaternion offsetRotation = Quaternion.Euler(0, 180, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation * offsetRotation, Time.deltaTime * 5f); 

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
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector3 shootDirection = (target.position - transform.position).normalized;
        projectile.GetComponent<Rigidbody>().velocity = shootDirection * 40f; 
    }
}
