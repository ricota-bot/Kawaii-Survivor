using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class MeleeEnemy : Enemy
{
    [Header("Attack")]
    [SerializeField] private int _damage;
    [SerializeField] private float _attackFrequency;
    private float _attackDelay; // Set this on in Inspector
    private float _attackTimer; // Use this to increment++ your Timer by Time.deltaTime .... if attackTicker >= attackDelay made somethinng..

    protected override void Start()
    {
        base.Start();
        _attackDelay = 1f / _attackFrequency; // This represent how much attack your deal by seconds if attaFrequency is 2 you make 2 attack per second...
    }

    private void Update()
    {
        if (!CanAttack())
            return;


        if (_attackTimer >= _attackDelay)
            EnemyNearlyToPlayer();
        else
        {
            Wait();
        }
    }

    private void EnemyNearlyToPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);

        if (distanceToPlayer <= playerDetectRadius)
            Attack();
    }

    private void Attack()
    {
        Debug.Log("Atack Atack Atack..... bAW BAW BAW");
        _attackTimer = 0;

        _player.TakeDamage(_damage);
    }
    private void Wait()
    {
        _attackTimer += Time.deltaTime;
    }
}