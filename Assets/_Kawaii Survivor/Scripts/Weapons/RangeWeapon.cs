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
        int damage = GetDamage(out bool isCriticalHit);
        //Vector2 direction = _shootingPoint.transform.right;
        PlayerBullet bulletInstance = _playerBulletPool.Get();
        bulletInstance.Shoot(damage, transform.up, isCriticalHit);
    }

    public override void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        ConfigureStats();

        _weaponDamage = Mathf.RoundToInt(_weaponDamage * (1 + playerStatsManager.GetStatValue(Stat.Attack) / 100));
        _attackDelay /= 1 + (playerStatsManager.GetStatValue(Stat.AttackSpeed) / 100);
        _criticalChance = Mathf.RoundToInt(_criticalChance * (1 + playerStatsManager.GetStatValue(Stat.CriticalChance) / 100));
        _criticalPercent += playerStatsManager.GetStatValue(Stat.CriticalPercent);

        _weaponRange += playerStatsManager.GetStatValue(Stat.Range) / 10; // Divide por 10 para não aumentar muito com o decorrer do tempo
    }
}

