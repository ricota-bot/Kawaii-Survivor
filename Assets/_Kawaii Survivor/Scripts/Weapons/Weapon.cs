using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    enum State { Idle, Attack } // MY ENUM
    private State _state;

    [Header("Elements")]
    [SerializeField] private Transform hitDetectionTransform;
    //[SerializeField] private BoxCollider2D _hitCollider; // Caso queira mudar o collider da melee ??

    [Header("Settings")]
    [SerializeField] protected float _weaponRange;
    [SerializeField] private float _hitDetectionRadius;
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

    private List<Enemy> _damagedEnemiesList = new List<Enemy>();
    private void Start()
    {
        _state = State.Idle;
    }

    private void Update()
    {
        switch (_state)
        {
            case State.Idle:
                AutoAim();
                break;

            case State.Attack:
                Attacking();
                break;
        }

    }

    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();

        Vector2 targetUpVector = Vector3.up;

        if (closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            transform.up = targetUpVector;
            ManageAttack();
        }

        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * _aimLerp);
        IncrementAttackTimer();
    }

    private void ManageAttack()
    {
        if (_attackTimer >= _attackDelay)
        {
            _attackTimer = 0f;
            StartAttack();
        }
    }

    private void IncrementAttackTimer()
    {
        _attackTimer += Time.deltaTime;
    }

    private void StartAttack()
    {
        _animator.Play("Attack");
        _state = State.Attack;

        _damagedEnemiesList.Clear();
        _animator.speed = 1f / _attackDelay;
    }

    private void Attacking()
    {
        Attack();
    }

    private void StopAttack()
    {
        _state = State.Idle;

        // Clear the Attacked Enemies
        _damagedEnemiesList.Clear();
    }

    private void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(hitDetectionTransform.position, _hitDetectionRadius, _layerMask);
        // Using List Now

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i].GetComponent<Enemy>();
            // 1. Is the enemy inside of the List ?
            if (!_damagedEnemiesList.Contains(enemy)) // Não esta dentro da Lista
            {
                enemy.TakeDamage(_weaponDamage);
                _damagedEnemiesList.Add(enemy); // Adicionamos esse Inimigo a Lista
                // Fazemos isso para não atacar o Player duas Vezes
            }
            // 2. If no let's attack him, and add it to the List

            // 3. If Yes, let's continue, check the next enemy
        }

    }

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

    private void OnDrawGizmos()
    {
        if (!displayGizmos)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _weaponRange);

        if (hitDetectionTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitDetectionTransform.position, _hitDetectionRadius);

        }
    }
}
