using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : MonoBehaviour
{
    public float damage = 10f;
    public float fireRate = 1f;
    public int maxAmmo = 10;
    public float reloadTime = 2f;
    public bool isAutomatic = false;
    public GameObject projectilePrefab;
    public float projectileSpeed = 20f;
    public AudioClip[] fireSounds;
    public AudioClip[] reloadSounds;

    private float nextTimeToFire = 0f;
    private int currentAmmo;
    private bool isReloading = false;

    private AudioSource audioSource;

    public Camera fpsCam;
    public GameObject muzzleFlashObject;
    public Transform muzzleTransform;

    

    void Start()
    {
        currentAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0 && Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            if (isAutomatic && Input.GetButton("Fire1"))
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                FireProjectile();
            }
            else if (!isAutomatic && Input.GetButtonDown("Fire1"))
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                FireProjectile();
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        PlayReloadSound();
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    void FireProjectile()
    {
        if (projectilePrefab && muzzleTransform)
        {
            GameObject projectile = Instantiate(projectilePrefab, muzzleTransform.position, muzzleTransform.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = muzzleTransform.forward * projectileSpeed;
            PlayGunFireSound();
            StartCoroutine(MuzzleFlashSequence()); // Activate muzzle flash
            currentAmmo--;
        }
        else
        {
            Debug.LogError("Projectile prefab or muzzle transform is not assigned.");
        }
    }

    IEnumerator MuzzleFlashSequence()
    {
        muzzleFlashObject.SetActive(true);
        yield return new WaitForSeconds(0.2f); 
        muzzleFlashObject.SetActive(false);
    }

    void PlayGunFireSound()
    {
        if (fireSounds != null && fireSounds.Length > 0)
        {
            AudioClip clipToPlay = fireSounds[Random.Range(0, fireSounds.Length)];
            if (clipToPlay != null)
            {
                audioSource.PlayOneShot(clipToPlay);
            }
            else
            {
                Debug.LogWarning("Attempted to play a null fire sound clip.");
            }
        }
    }

    void PlayReloadSound()
    {
        if (reloadSounds != null && reloadSounds.Length > 0)
        {
            AudioClip clipToPlay = reloadSounds[Random.Range(0, reloadSounds.Length)];
            if (clipToPlay != null)
            {
                audioSource.PlayOneShot(clipToPlay);
            }
            else
            {
                Debug.LogWarning("Attempted to play a null reload sound clip.");
            }
        }
    }
}
