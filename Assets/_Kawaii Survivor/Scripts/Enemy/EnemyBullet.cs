using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyBullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D _rig;
    private Collider2D _bulletCollider;
    private RangeEnemyAttack _rangeEnemyAttack;

    [Header("Settings")]
    [SerializeField] private float _bulletSpeed;
    private int _bulletDamage; // Receive damage from another script


    private void Awake()
    {
        _rig = GetComponent<Rigidbody2D>();
        _bulletCollider = GetComponent<Collider2D>();

        LeanTween.delayedCall(this.gameObject, 5, () => _rangeEnemyAttack.ReleaseBullet(this));
    }

    public void Configure(RangeEnemyAttack rangeEnemyAttack)
    {
        _rangeEnemyAttack = rangeEnemyAttack;
    }
    public void Shoot(int damage, Vector2 direction)
    {
        _bulletDamage = damage; // Receive damage value in is Reference "RangeEnemyAttack Class"
        transform.right = direction; // Define a Direção
        _rig.linearVelocity = direction * _bulletSpeed;
    }


    public void Reload()
    {
        _rig.linearVelocity = Vector2.zero;
        _bulletCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            LeanTween.cancel(this.gameObject); // Caso acertar o Player vamos cancelar o LeanTween de dar Release na bullets
            player.TakeDamage(_bulletDamage);
            _bulletCollider.enabled = false;
            _rangeEnemyAttack.ReleaseBullet(this); // Quando usamos o Release Desativamos o GameObject
        }
    }
}
