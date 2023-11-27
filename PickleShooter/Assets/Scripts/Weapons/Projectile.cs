using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f; // Lifetime of the projectile in seconds
    public float damage = 10f;
    public GameObject impactEffect;


    void Start()
    {
        // Destroy the projectile after its lifetime has elapsed
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check the tag of the object the projectile collides with
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Pickle"))
        {
            collision.gameObject.GetComponent<Target>().TakeDamage(damage);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
        else
        {
            Debug.Log("Hit something else: " + collision.gameObject.tag);
        }

        // Create impact effect at the point of collision
        if (impactEffect != null)
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rotation = Quaternion.LookRotation(contact.normal);
            GameObject impactGO = Instantiate(impactEffect, contact.point, rotation);
            Destroy(impactGO, 2f); // Destroy the impact effect after 2 seconds
        }

        // Destroy the projectile on collision
        Destroy(gameObject);
    }
}
