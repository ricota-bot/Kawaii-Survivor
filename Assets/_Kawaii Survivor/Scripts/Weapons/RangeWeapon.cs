using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class RangeWeapon : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private PlayerBullet _bulletPrefab;
    private Enemy _enemy;

    [Header("Settings")]
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _weaponRange;
    [SerializeField] private int _weaponDamage;
    [Space(15)]
    [SerializeField] private float _attackDelay;
    private float _attackTimer; // Use this to increment++ your Timer by Time.deltaTime .... if attackTicker >= attackDelay made somethinng..
    private float _attackFrequency;

    [Header("Animations")]
    [SerializeField] private float _aimLerp;

    [Header("Debug")]
    [SerializeField] private bool displayGizmos;

    private void Update()
    {
        AutoAim();
    }

    private Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _weaponRange, _layerMask);

        if (enemies.Length <= 0)
            return null;


        float minDistance = _weaponRange;

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemyChecked = enemies[i].GetComponent<Enemy>();

            float distanceToEnemy = Vector2.Distance(transform.position, enemyChecked.transform.position);

            if (distanceToEnemy < minDistance)
            {
                closestEnemy = enemyChecked;
                minDistance = distanceToEnemy;
            }
        }

        return closestEnemy;
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
        Debug.Log("Shoot!!");
        ShootingBullets();
    }

    private void ShootingBullets()
    {
        Debug.Log("Shoo2t!!");

        // Instantiate Bullet
        //Vector2 direction = ((Vector2)_enemy.transform.position - (Vector2)_shootingPoint.position).normalized;
        Vector2 direction = _shootingPoint.transform.right;
        PlayerBullet bulletInstance = Instantiate(_bulletPrefab, _shootingPoint.position, Quaternion.identity);
        bulletInstance.Shoot(_weaponDamage, direction);
    }

    private void OnDrawGizmos()
    {
        if (!displayGizmos)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _weaponRange);
    }
}

