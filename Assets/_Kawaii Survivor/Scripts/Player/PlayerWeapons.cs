using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private WeaponPosition[] weaponsPositions;

    public void AddWeapons(WeaponDataSO selectedWeapon, int weaponLevel)
    {
        //Instantiate(selectedWeapon.Prefab, weaponsParent);
        weaponsPositions[Random.Range(0, weaponsPositions.Length)].AssignWeapon(selectedWeapon.Prefab, weaponLevel);

    }
}
