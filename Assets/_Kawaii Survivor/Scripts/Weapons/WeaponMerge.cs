using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMerge : MonoBehaviour
{
    public static WeaponMerge instance;

    [Header("Elements")]
    [SerializeField] private PlayerWeapons playerweapons;
    private List<Weapon> weaponsToMerge = new List<Weapon>();

    [Header("Action")]
    public static Action<Weapon> onMerge;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public bool CanMerge(Weapon weapon)
    {
        if (weapon.Level >= 3)
            return false;

        weaponsToMerge.Clear();
        weaponsToMerge.Add(weapon); // Add the weapon your select to list to merge weapons

        Weapon[] weapons = playerweapons.GetWeapons();

        foreach (Weapon playerWeapon in weapons)
        {
            // We can't merge with a null Weapons 
            if (playerWeapon == null)
                continue;

            // We can't merge a weapon with itself
            if (playerWeapon == weapon)
                continue;

            // Not the same weapons
            if (playerWeapon.WeaponData.WeaponName != weapon.WeaponData.WeaponName)
                continue;

            // We can't merge "same" weapons with different levels
            if (playerWeapon.Level != weapon.Level)
                continue;

            weaponsToMerge.Add(playerWeapon); // If you pass for all add This Weapon to the List

            return true;
        }

        return false;
    }

    public void Merge()
    {
        if (weaponsToMerge.Count < 2)
        {
            Debug.LogError("Somenthing went wrong here...");
            return;
        }

        DestroyImmediate(weaponsToMerge[1].gameObject); // Remove one weapons

        weaponsToMerge[0].Upgrade(); // Upgrade the other Weapon

        Weapon weapon = weaponsToMerge[0];
        weaponsToMerge.Clear();

        onMerge?.Invoke(weapon);
    }
}
