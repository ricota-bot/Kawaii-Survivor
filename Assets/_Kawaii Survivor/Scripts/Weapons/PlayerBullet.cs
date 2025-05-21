using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerBullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D _rig;
    private Collider2D _bulletCollider;
    //private RangeEnemyAttack _rangeEnemyAttack;

    [Header("Settings")]
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private float _bulletSpeed;
    private int _bulletDamage; // Receive damage from another script
    private bool _isCriticalHit;

    private RangeWeapon _rangeWeapon;

    private Enemy _targetEnemy;

    private void Awake()
    {
        _rig = GetComponent<Rigidbody2D>();
        _bulletCollider = GetComponent<Collider2D>();
    }

    public void Configure(RangeWeapon rangeWeapon)
    {
        _rangeWeapon = rangeWeapon;
    }

    public void Reload()
    {
        _targetEnemy = null;
        _rig.linearVelocity = Vector2.zero;
        _bulletCollider.enabled = true;
    }

    private void Release()
    {
        if (!gameObject.activeSelf)
            return;
        _rangeWeapon.ReleaseBullet(this);
    }

    public void Shoot(int damage, Vector2 direction, bool isCriticalHit)
    {
        Invoke("Release", 1);
        _isCriticalHit = isCriticalHit;
        _bulletDamage = damage;
        transform.right = direction;
        _rig.linearVelocity = direction * _bulletSpeed;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_targetEnemy != null) // Caso já tenhamos um Target apenas retornamos
            return;

        if (IsInLayerMask(collision.gameObject.layer, _enemyMask))
        {
            _targetEnemy = collision.GetComponent<Enemy>();
            CancelInvoke();
            Attack(_targetEnemy);
            Release();
        }
    }

    private void Attack(Enemy enemy)
    {
        enemy.TakeDamage(_bulletDamage, _isCriticalHit);
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0;
    }
}
