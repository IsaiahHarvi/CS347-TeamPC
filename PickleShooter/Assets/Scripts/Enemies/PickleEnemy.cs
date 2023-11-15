using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PickleEnemy : MonoBehaviour
{
    // Audio
    public AudioClip[] soundEffects;
    private AudioSource audioSource;
    private float timeSinceLastSound = 0f;

    // Pathfinding
    private NavMeshAgent agent;
    public Transform target; 

    void Start()
    {
        // Initialize or add audio source
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Initialize NavMeshAgent
        agent = GetComponent<NavMeshAgent>();

        // Set target to a GameObject with tag 'Target' if target is null
        if (target == null)
        {
            GameObject targetObject = GameObject.FindGameObjectWithTag("Target");
            if (targetObject != null)
            {
                target = targetObject.transform;
            }
            else
            {
                Debug.LogError("No GameObject with tag 'Target' found in the scene.");
            }
        }
    }

    void Update()
    {
        // Pathfinding logic
        if (target != null)
        {
            agent.SetDestination(target.position);

            // Calculate direction to target
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // Ensure the rotation is only on the Y axis

            // Check if direction is not zero
            if (direction != Vector3.zero)
            {
                // Calculate rotation towards the target
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Apply a 90 degrees offset on Y axis
                Quaternion offsetRotation = Quaternion.Euler(0, 90, 0);
                targetRotation *= offsetRotation;

                // Set the rotation of the pickle
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * agent.angularSpeed);
            }

            timeSinceLastSound += Time.deltaTime;

            if (timeSinceLastSound >= 8f)
            {
                PlayRandomSound();
                timeSinceLastSound = 0f;
            }
        }
        else
        {
            Debug.LogWarning("Target is null.");
        }
    }

    private void PlayRandomSound()
    {
        if (soundEffects.Length > 0)
        {
            int index = Random.Range(0, soundEffects.Length);
            audioSource.PlayOneShot(soundEffects[index], 1.0f);
        }
    }
}
