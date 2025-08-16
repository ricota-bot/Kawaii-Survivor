using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private WeaponPosition[] weaponsPositions;

    public bool TryAddWeapons(WeaponDataSO weaponData, int weaponLevel)
    {
        for (int i = 0; i < weaponsPositions.Length; i++)
        {
            if (weaponsPositions[i].Weapon != null)
                continue; // J� possui uma arma nesta posi��o

            // Se a posi��o estiver vazia, instancie a arma
            weaponsPositions[i].AssignWeapon(weaponData.Prefab, weaponLevel);
            return true;
        }

        return false;
    }

    public void RecycleWeapon(int weaponIndex)
    {
        for (int i = 0; i < weaponsPositions.Length; i++)
        {
            if (weaponIndex != i)
                continue; // N�o � o Index que estamos procurando

            // Caso dar match nos dois Index, vamos remover essa arma
            int recyclePrice = weaponsPositions[i].Weapon.GetRecyclePrice(); // Pegamos o pre�o da arma
            CurrencyManager.Instance.AddCurrency(recyclePrice); // Adicionamos esse valor no nosso CurrencyManager
            weaponsPositions[i].RemoveWeapon(); // Removemos a Arma

            return; // Retornamos pois j� encontramos o que queria :)
        }
    }


    public Weapon[] GetWeapons()
    {
        List<Weapon> weaponsList = new List<Weapon>();

        foreach (WeaponPosition weaponPosition in weaponsPositions)
        {
            if (weaponPosition.Weapon == null)
                weaponsList.Add(null); // Adicionamos "Null" , para ter pelo menos um valor nesse Index ex: "index 2" == null 
            else
                weaponsList.Add(weaponPosition.Weapon);
        }

        return weaponsList.ToArray(); // Adicionando um Null na nossa lista, sempre vamos ter os 6 dentro dessa lista
        // Porem os valores que n�o tem nada, vai ser preenchido como Null
    }


}
