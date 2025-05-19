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

    private void Awake()
    {
        _rig = GetComponent<Rigidbody2D>();
        _bulletCollider = GetComponent<Collider2D>();
    }

    public void Shoot(int damage, Vector2 direction)
    {
        _bulletDamage = damage;
        transform.right = direction;
        _rig.linearVelocity = direction * _bulletSpeed;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsInLayerMask(collision.gameObject.layer, _enemyMask))
        {
            Attack(collision.GetComponent<Enemy>());
            Destroy(this.gameObject);
        }
    }

    private void Attack(Enemy enemy)
    {
        enemy.TakeDamage(_bulletDamage);
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0;
    }
}
