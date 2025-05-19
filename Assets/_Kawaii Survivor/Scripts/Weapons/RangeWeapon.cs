using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class RangeWeapon : Weapon
{
    [Header("Elements")]
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private PlayerBullet _bulletPrefab;
    private Enemy _enemy;

    private void Update()
    {
        AutoAim();
    }

    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();

        Vector2 targetUpVector = Vector3.up;

        if (closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            transform.up = targetUpVector;
            ManageShooting();
            return;
        }

        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * _aimLerp);
    }

    private void ManageShooting()
    {
        _attackTimer += Time.deltaTime;

        if (_attackTimer >= _attackDelay)
        {
            _attackTimer = 0f;
            Shoot();
        }
    }

    private void Shoot()
    {
        _animator.Play("Shoot");

        // Instantiate Bullet
        //Vector2 direction = ((Vector2)_enemy.transform.position - (Vector2)_shootingPoint.position).normalized;
        Vector2 direction = _shootingPoint.transform.right;
        PlayerBullet bulletInstance = Instantiate(_bulletPrefab, _shootingPoint.position, Quaternion.identity);
        bulletInstance.Shoot(_weaponDamage, direction);
    }
}

