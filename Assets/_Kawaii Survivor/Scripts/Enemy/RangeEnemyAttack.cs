using System;
using UnityEngine;
using UnityEngine.Pool;

public class RangeEnemyAttack : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private EnemyBullet _bulletPrefab;
    private Player _player;

    [Header("Attack Settings")]
    [SerializeField] private int _damage;
    [SerializeField] private float _attackFrequency;
    private float _attackDelay; // Set this on in Inspector
    private float _attackTimer; // Use this to increment++ your Timer by Time.deltaTime .... if attackTicker >= attackDelay made somethinng

    [Header("Pooling")]
    private ObjectPool<EnemyBullet> _bulletPool;

    private void Start()
    {
        _attackDelay = 1f / _attackFrequency; // This represent how much attack your deal by seconds if attackFrequency is 2 you make 2 attack per second...
        _attackTimer = _attackDelay;

        _bulletPool = new ObjectPool<EnemyBullet>(
            CreateFunc,
            ActionOnGet,
            ActionOnRelease,
            ActionOnDestroy);
    }

    #region Object Pooling
    private EnemyBullet CreateFunc()
    {
        EnemyBullet bulletInstance = Instantiate(_bulletPrefab, _shootingPoint.position, Quaternion.identity);
        bulletInstance.Configure(this);
        return bulletInstance;
    }
    private void ActionOnGet(EnemyBullet bullet)
    {
        bullet.Reload();
        bullet.transform.position = _shootingPoint.position;
        bullet.gameObject.SetActive(true);
    }

    private void ActionOnRelease(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(EnemyBullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public void ReleaseBullet(EnemyBullet bullet) =>
        _bulletPool.Release(bullet);


    #endregion

    public void StorePlayer(Player player) => _player = player;

    public void AutoAim()
    {
        ManageShooting();
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
        Vector2 direction = (_player.GetCenter() - (Vector2)_shootingPoint.position).normalized;

        EnemyBullet bulletInstance = _bulletPool.Get();

        bulletInstance.Shoot(_damage, direction);

    }


}
