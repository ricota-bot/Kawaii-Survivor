using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    enum State { Idle, Attack } // MY ENUM
    private State _state;

    [Header("Elements")]
    [SerializeField] private Transform hitDetectionTransform;
    //[SerializeField] private BoxCollider2D _hitCollider; // Caso queira mudar o collider da melee ??
    [SerializeField] private float _hitDetectionRadius;

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
        Collider2D[] enemies = Physics2D.OverlapCircleAll
            (
            hitDetectionTransform.position,
            _hitDetectionRadius,
            _layerMask
            );

        // Using List Now

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i].GetComponent<Enemy>();

            if (!_damagedEnemiesList.Contains(enemy)) // Não esta dentro da Lista
            {
                enemy.TakeDamage(_weaponDamage);
                _damagedEnemiesList.Add(enemy); // Adicionamos esse Inimigo a Lista
                // Fazemos isso para não atacar o Player duas Vezes
            }
        }

    }
}
