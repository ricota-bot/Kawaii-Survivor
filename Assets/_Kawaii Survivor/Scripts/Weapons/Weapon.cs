using NaughtyAttributes;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IPlayerStatsDepedency
{
    [Header("Data")]
    [Expandable]
    [SerializeField] private WeaponDataSO _weaponData;
    public WeaponDataSO WeaponData { get => _weaponData; private set => _weaponData = value; }

    [Header("Level")]
    public int Level { get; private set; }

    [Header("Settings")]
    [SerializeField] protected float _weaponRange;
    [SerializeField] protected LayerMask _layerMask;

    [Header("Attack")]
    [SerializeField] protected int _weaponDamage;
    [SerializeField] protected float _attackDelay;
    protected float _attackTimer; // Use this to increment++ your Timer by Time.deltaTime .... if attackTicker >= attackDelay made something..

    [Header("Critical")]
    [SerializeField] protected int _criticalChance;
    [SerializeField] protected float _criticalPercent;

    [Header("Animations")]
    [SerializeField] protected Animator _animator;
    [SerializeField] protected float _aimLerp;

    [Header("Debug")]
    [SerializeField] protected bool displayGizmos;

    protected Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _weaponRange, _layerMask);

        if (enemies.Length <= 0)
            return null;


        float minDistance = _weaponRange;

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemyChecked = enemies[i].GetComponent<Enemy>();

            float distanceToEnemy = Vector2.Distance(transform.position, enemyChecked.transform.position);

            if (distanceToEnemy < minDistance)
            {
                closestEnemy = enemyChecked;
                minDistance = distanceToEnemy;
            }
        }

        return closestEnemy;
    }
    protected int GetDamage(out bool isCriticalHit)
    {
        isCriticalHit = false;

        if (Random.Range(0, 101) <= _criticalChance)
        {
            isCriticalHit = true;
            return Mathf.RoundToInt(_weaponDamage * _criticalPercent);
        }

        return _weaponDamage;
    }
    protected void ConfigureStats()
    {
        float multiplier = 1 + (float)Level / 3;
        _weaponDamage = Mathf.RoundToInt(WeaponData.GetStatValue(Stat.Attack) * multiplier);

        _attackDelay = 1f / (WeaponData.GetStatValue(Stat.AttackSpeed) * multiplier);

        _criticalChance = Mathf.RoundToInt(WeaponData.GetStatValue(Stat.CriticalChance) * multiplier);
        _criticalPercent = WeaponData.GetStatValue(Stat.CriticalPercent) * multiplier;

        if (WeaponData.Prefab.GetType() == typeof(RangeWeapon))
        {
            _weaponRange = WeaponData.GetStatValue(Stat.Range) * multiplier;
        }
    }

    public abstract void UpdateStats(PlayerStatsManager playerStatsManager);


    public void UpgradeTo(int targetLevel)
    {
        Level = targetLevel;
        ConfigureStats();
    }


    private void OnDrawGizmos()
    {
        if (!displayGizmos)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _weaponRange);
    }
}
