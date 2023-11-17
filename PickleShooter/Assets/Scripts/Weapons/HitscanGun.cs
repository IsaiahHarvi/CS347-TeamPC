using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HitscanGun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;
    public int maxAmmo = 30;
    public float reloadTime = 1f;
    public bool isAutomatic = false;
    public AudioClip[] fireSounds;
    public AudioClip[] reloadSounds;


    private float nextTimeToFire = 0f;
    private int currentAmmo;
    private bool isReloading = false;

    private AudioSource audioSource;

    public Camera fpsCam;
    public GameObject muzzleFlashObject;
    public GameObject impactEffect;

    public float recoilAmount = 5f; // Adjust this value to control the recoil strength
    public float recoilRecoverySpeed = 5f; // Speed at which the weapon returns to original position

    private Vector3 originalRotation; // To store the original rotation of the weapon
    private float currentRecoilOffset = 0f; // Current recoil offset on X-axis

    public TextMeshProUGUI ammoText;

    void Start()
    {
        currentAmmo = maxAmmo;
        muzzleFlashObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();

        originalRotation = transform.localEulerAngles;
    }

    void OnEnable()
    {
        isReloading = false;
    }

    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0 || Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        // Check if enough time has passed based on the fire rate
        if (Time.time >= nextTimeToFire)
        {
            if (isAutomatic && Input.GetButton("Fire1"))
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
            else if (!isAutomatic && Input.GetButtonDown("Fire1"))
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
        }

        // Apply recoil recovery
        currentRecoilOffset = Mathf.Lerp(currentRecoilOffset, 0f, recoilRecoverySpeed * Time.deltaTime);
        ApplyRecoil();

        ammoText.text = currentAmmo + " / " + maxAmmo;
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


    void Shoot()
    {
        StartCoroutine(MuzzleFlashSequence());
        PlayGunFireSound();
        currentAmmo--;

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }

        // Apply recoil
        currentRecoilOffset += recoilAmount; // Increase recoil offset on X-axis
        ApplyRecoil();
    }

    IEnumerator MuzzleFlashSequence()
    {
        muzzleFlashObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        muzzleFlashObject.SetActive(false);
    }

    void PlayGunFireSound()
    {
        if (fireSounds.Length > 0)
        {
            // Play a random sound from the array
            audioSource.PlayOneShot(fireSounds[Random.Range(0, fireSounds.Length)]);
        }
    }

    void PlayReloadSound()
    {
        if (reloadSounds.Length > 0)
        {
            // Play a random sound from the array
            audioSource.PlayOneShot(reloadSounds[Random.Range(0, reloadSounds.Length)]);
        }
    }

    void ApplyRecoil()
    {
        // Apply the current recoil to the weapon's rotation
        transform.localEulerAngles = new Vector3(originalRotation.x + currentRecoilOffset, originalRotation.y, originalRotation.z);
    }

}
