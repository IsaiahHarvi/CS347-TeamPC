using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private float nextTimeToFire = 0f;
    private int currentAmmo;
    private bool isReloading = false;

    private AudioSource audioSource;

    public Camera fpsCam;
    public GameObject muzzleFlashObject;
    public GameObject impactEffect;

    void Start()
    {
        currentAmmo = maxAmmo;
        muzzleFlashObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
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

        if (isAutomatic && Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
        else if (!isAutomatic && Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

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
}
