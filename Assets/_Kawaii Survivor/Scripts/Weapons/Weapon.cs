using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] protected float _weaponRange;
    [SerializeField] protected LayerMask _layerMask;

    [Header("Attack")]
    [SerializeField] protected int _weaponDamage;
    [SerializeField] protected float _attackDelay;
    protected float _attackTimer; // Use this to increment++ your Timer by Time.deltaTime .... if attackTicker >= attackDelay made somethinng..

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

        int chanceToCritical = 50;
        if (Random.Range(0, 101) <= chanceToCritical)
        {
            isCriticalHit = true;
            return _weaponDamage * 2;
        }

        return _weaponDamage;
    }

    private void OnDrawGizmos()
    {
        if (!displayGizmos)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _weaponRange);
    }
}
