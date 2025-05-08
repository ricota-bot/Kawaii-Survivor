using TMPro;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [Header("Elements")]
    private Player _player;
    private EnemyMovement _enemyMovement;

    [Header("Enemy Health")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private TextMeshPro _healthText;
    private int _health;

    [Header("Spawn Sequence Related")]
    [SerializeField] private SpriteRenderer _enemyRenderer;
    [SerializeField] private SpriteRenderer _spawnIndicatorRenderer;


    [Header("Detection Radius")]
    [SerializeField] private float playerDetectRadius;

    [Header("Debug")]
    [SerializeField] private bool displayGizmos;

    [Header("Particles")]
    [SerializeField] private ParticleSystem _particleSystem;

    [Header("Attack")]
    [SerializeField] private int _damage;
    [SerializeField] private float _attackFrequency;
    private float _attackDelay; // Set this on in Inspector
    private float _attackTimer; // Use this to increment++ your Timer by Time.deltaTime .... if attackTicker >= attackDelay made somethinng..


    private void Start()
    {
        _player = FindFirstObjectByType<Player>();
        _enemyMovement = GetComponent<EnemyMovement>();

        _health = _maxHealth;
        _healthText.text = _health.ToString();

        if (_player == null)
            Destroy(gameObject);

        StartSpawnSequence();

        _attackDelay = 1f / _attackFrequency; // This represent how much attack your deal by seconds if attaFrequency is 2 you make 2 attack per second...
    }

    private void Update()
    {
        if (_attackTimer >= _attackDelay)
            EnemyNearlyToPlayer();
        else
        {
            Wait();
        }
    }

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

    private void EnemyNearlyToPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);

        if (distanceToPlayer <= playerDetectRadius)
            Attack();
    }

    private void Attack()
    {
        Debug.Log("Atack Atack Atack..... bAW BAW BAW");
        _attackTimer = 0;

        _player.TakeDamage(_damage);
    }
    private void Wait()
    {
        _attackTimer += Time.deltaTime;
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

        _healthText.text = _health.ToString();

        if (_health <= 0)
            PassWay();
    }

    private void OnDrawGizmos()
    {
        if (!displayGizmos)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, playerDetectRadius);
    }
}
