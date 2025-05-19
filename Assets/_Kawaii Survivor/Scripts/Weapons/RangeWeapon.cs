using System;
using UnityEngine;
using UnityEngine.Pool;

public class RangeWeapon : Weapon
{
    [Header("Elements")]
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private PlayerBullet _bulletPrefab;
    private Enemy _enemy;

    [Header("Pooling")]
    private ObjectPool<PlayerBullet> _playerBulletPool;
    private void Start()
    {
        _playerBulletPool = new ObjectPool<PlayerBullet>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }
    private void Update()
    {
        AutoAim();
    }
    private PlayerBullet CreateFunc()
    {
        PlayerBullet playerBulletInstance = Instantiate(_bulletPrefab, _shootingPoint.position, Quaternion.identity);
        playerBulletInstance.Configure(this);
        return playerBulletInstance;
    }

    private void ActionOnGet(PlayerBullet playerBullet)
    {
        playerBullet.Reload();
        playerBullet.transform.position = _shootingPoint.position;
        playerBullet.gameObject.SetActive(true);
    }

    private void ActionOnRelease(PlayerBullet playerBullet)
    {
        playerBullet.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(PlayerBullet playerBullet)
    {
        Destroy(playerBullet.gameObject);
    }

    public void ReleaseBullet(PlayerBullet playerBullet)
    {
        _playerBulletPool.Release(playerBullet);
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
        PlayerBullet bulletInstance = _playerBulletPool.Get();
        bulletInstance.Shoot(_weaponDamage, direction);
    }
}

