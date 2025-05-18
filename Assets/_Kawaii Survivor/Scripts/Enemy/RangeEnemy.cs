using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement), typeof(RangeEnemyAttack))]
public class RangeEnemy : MonoBehaviour
{
    [Header("Elements")]
    private Player _player;
    private EnemyMovement _enemyMovement;
    private RangeEnemyAttack _rangeEnemyAttack;

    [Header("Enemy Health")]
    [SerializeField] private int _maxHealth;
    private int _health;

    [Header("Spawn Sequence Related")]
    [SerializeField] private SpriteRenderer _enemyRenderer;
    [SerializeField] private SpriteRenderer _spawnIndicatorRenderer;
    [SerializeField] private Collider2D _enemyCollider;

    [Header("Particles")]
    [SerializeField] private ParticleSystem _particleSystem;

    [Header("Detection Radius")]
    [SerializeField] private float playerDetectRadius;


    [Header("Actions")]
    public static Action<Vector2, int> OnDamageTaken;

    [Header("Debug")]
    [SerializeField] private bool displayGizmos;



    private void Start()
    {
        _player = FindFirstObjectByType<Player>();
        _enemyMovement = GetComponent<EnemyMovement>();
        _rangeEnemyAttack = GetComponent<RangeEnemyAttack>();

        _rangeEnemyAttack.StorePlayer(_player);


        _health = _maxHealth;

        if (_player == null)
            Destroy(gameObject);

        StartSpawnSequence();
    }


    private void Update()
    {
        if (!_enemyRenderer.enabled)
            return;

        ManageAttack();
    }

    #region Spawn Sequence
    private void StartSpawnSequence()
    {
        SetEnemyRendererVisibility(false);
        // Scale up & Scale Down the Spawn Indicator using Tween Library
        // After 4 seconds Show the Enemy Renderer
        Vector3 targetScale = _spawnIndicatorRenderer.transform.localScale * 1.2f;
        LeanTween.scale(_spawnIndicatorRenderer.gameObject, targetScale, 0.3f)
            .setLoopPingPong(4)
            .setOnComplete(SpawnSequenceCompleted);
    }
    private void SpawnSequenceCompleted()
    {
        SetEnemyRendererVisibility();
        // Set Spawned to True to move the Enemy Again
        //_hasSpawned = true;
        _enemyCollider.enabled = true;
        _enemyMovement.StorePlayer(_player);

    }

    /// <summary>
    /// If you set true, active _enemyRenderer and Disable _spawnIndicatorRenderer.
    /// </summary>
    private void SetEnemyRendererVisibility(bool visibility = true)
    {
        // Hide Enemy Renderer
        _enemyRenderer.enabled = visibility;
        // Show the Spawn Indicator  (Prevent Following & Attacking during the Spawn Sequence) Velocity equals ZERO ?
        _spawnIndicatorRenderer.enabled = !visibility;
    }

    #endregion

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

    public void PassWay()
    {
        _particleSystem.transform.parent = null;
        _particleSystem.Play();
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, _health);

        _health -= realDamage;

        if (_health <= 0)
            PassWay();

        OnDamageTaken?.Invoke(transform.position, realDamage);
    }

    #region Gizmos
    private void OnDrawGizmos()
    {
        if (!displayGizmos)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, playerDetectRadius);
    }

    #endregion
}
