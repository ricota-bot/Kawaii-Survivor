using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement), typeof(RangeEnemyAttack))]
public class RangeEnemy : Enemy
{
    [Header("Elements")]
    private RangeEnemyAttack _rangeEnemyAttack;

    protected override void Start()
    {
        base.Start();

        _rangeEnemyAttack = GetComponent<RangeEnemyAttack>();
        _rangeEnemyAttack.StorePlayer(_player);

    }

    private void Update()
    {
        if (!CanAttack())
            return;

        ManageAttack();

        transform.localScale = _player.transform.position.x > transform.position.x ?
            Vector3.one :
            Vector3.one.With(x: -1);
    }

    private void ManageAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);

        if (distanceToPlayer > playerDetectRadius)
        {
            _enemyMovement.MoveKeepDistance();
        }
        else
        {
            TryAttack();
        }
    }
    private void TryAttack()
    {
        _rangeEnemyAttack.AutoAim();
        Debug.Log("Tento atacar!");
    }
}
