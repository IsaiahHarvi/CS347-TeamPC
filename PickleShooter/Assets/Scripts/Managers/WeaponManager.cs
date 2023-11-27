using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] weapons;
    private int currentWeaponIndex;

    void Start()
    {
        InitializeWeapons();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && CanSwitchWeapon())
        {
            SwitchWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && CanSwitchWeapon())
        {
            SwitchWeapon(1);
        }
        // Add more if statements for additional weapons
    }

    private bool CanSwitchWeapon()
    {
        IWeapon currentWeaponScript = weapons[currentWeaponIndex].GetComponent<IWeapon>();
        if (currentWeaponScript != null)
        {
            return !currentWeaponScript.IsReloading();
        }
        return true; // If the weapon doesn't implement IWeapon, assume it can be switched
    }

    private void InitializeWeapons()
    {
        foreach (var weapon in weapons)
        {
            weapon.SetActive(false);
        }

        if (weapons.Length > 0)
        {
            weapons[0].SetActive(true);
            currentWeaponIndex = 0;
        }
    }

    private void SwitchWeapon(int index)
    {
        if (index >= 0 && index < weapons.Length && index != currentWeaponIndex)
        {
            weapons[currentWeaponIndex].SetActive(false);
            currentWeaponIndex = index;
            weapons[currentWeaponIndex].SetActive(true);
        }
    }
}
