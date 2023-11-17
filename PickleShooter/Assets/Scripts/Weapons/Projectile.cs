using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f; // Lifetime of the projectile in seconds
    public float damage = 10f;

    void Start()
    {
        // Destroy the projectile after its lifetime has elapsed
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check the tag of the object the projectile collides with
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Target>().TakeDamage(damage);
        }
        else
        {
            Debug.Log("Hit something else: " + collision.gameObject.tag);
        }

        // Destroy the projectile on collision
        Destroy(gameObject);
    }
}
