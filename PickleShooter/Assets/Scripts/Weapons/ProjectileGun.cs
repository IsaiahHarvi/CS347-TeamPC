using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProjectileGun : MonoBehaviour, IWeapon
{
    public float damage = 10f;
    public float fireRate = 1f;
    public int maxAmmo = 10;
    public float reloadTime = 1.5f;
    public bool isAutomatic = false;
    public GameObject projectilePrefab;
    public float projectileSpeed = 20f;
    public AudioClip[] fireSounds;
    public AudioClip[] reloadSounds;

    private float nextTimeToFire = 0f;
    private int currentAmmo;
    private bool isReloading = false;

    private AudioSource fireAudioSource;
    private AudioSource reloadAudioSource;

    public Camera fpsCam;
    public GameObject muzzleFlashObject;
    public Transform muzzleTransform;

    public TextMeshProUGUI ammoText;

    void Start()
    {
        currentAmmo = maxAmmo;
        AssignAudioSources();
    }

    void Update()
    {
        if (isReloading)
            return;

        HandleReloadInput();
        HandleFireInput();
        UpdateAmmoText();
    }

    private void AssignAudioSources()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length >= 2)
        {
            fireAudioSource = audioSources[0];
            reloadAudioSource = audioSources[1];
        }
        else
        {
            Debug.LogError("Not enough audio sources found on the GameObject");
        }
    }

    private void HandleReloadInput()
    {
        if (currentAmmo < maxAmmo && Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    private void HandleFireInput()
    {
        if (Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            bool fireButtonPressed = isAutomatic ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");
            if (fireButtonPressed)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                FireProjectile();
            }
        }
    }

    private void UpdateAmmoText()
    {
        ammoText.text = $"{currentAmmo} / {maxAmmo}";
    }

    IEnumerator Reload()
    {
        if (isReloading) yield break;

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
            StartCoroutine(MuzzleFlashSequence());
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
        PlaySound(fireAudioSource, fireSounds);
    }

    void PlayReloadSound()
    {
        PlaySound(reloadAudioSource, reloadSounds);
    }

    private void PlaySound(AudioSource source, AudioClip[] clips)
    {
        if (clips != null && clips.Length > 0)
        {
            AudioClip clipToPlay = clips[Random.Range(0, clips.Length)];
            if (clipToPlay != null)
            {
                source.PlayOneShot(clipToPlay);
            }
            else
            {
                Debug.LogWarning("Attempted to play a null audio clip.");
            }
        }
    }

    public bool IsReloading()
    {
        // Return the reloading state
        return isReloading;
    }
}
