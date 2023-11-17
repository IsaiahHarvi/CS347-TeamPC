using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] weapons; // Array to hold all weapon GameObjects
    private int currentWeaponIndex; // To keep track of the currently active weapon

    void Start()
    {
        InitializeWeapons();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Switch to weapon 1
        {
            SwitchWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // Switch to weapon 2
        {
            SwitchWeapon(1);
        }
        // Add more if statements for additional weapons

        // Additional input checks can be added here for other weapon switching methods
    }

    private void InitializeWeapons()
    {
        // Deactivate all weapons at start
        foreach (var weapon in weapons)
        {
            weapon.SetActive(false);
        }

        // Activate the first weapon by default
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
            weapons[currentWeaponIndex].SetActive(false); // Deactivate current weapon
            currentWeaponIndex = index;
            weapons[currentWeaponIndex].SetActive(true); // Activate new weapon
        }
    }
}
