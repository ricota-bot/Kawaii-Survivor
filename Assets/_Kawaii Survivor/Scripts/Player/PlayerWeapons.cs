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
}
