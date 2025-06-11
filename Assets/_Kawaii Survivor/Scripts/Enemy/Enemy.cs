using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Elements")]
    protected EnemyMovement _enemyMovement;
    protected Player _player;

    [Header("Enemy Health")]
    [SerializeField] protected int _maxHealth;
    protected int _health;

    [Header("Spawn Sequence Related")]
    [SerializeField] protected SpriteRenderer _enemyRenderer;
    [SerializeField] protected SpriteRenderer _spawnIndicatorRenderer;
    [SerializeField] protected Collider2D _enemyCollider;
    protected bool hasSpawned;

    [Header("Detection Radius")]
    [SerializeField] protected float playerDetectRadius;

    [Header("Particles")]
    [SerializeField] protected ParticleSystem _particleSystem;

    [Header("Actions")]
    public static Action<Vector2, int, bool> OnDamageTaken;
    public static Action<Vector2> OnEnemyPassWay;

    [Header("Debug")]
    [SerializeField] protected bool displayGizmos;

    protected virtual void Start()
    {
        _player = FindFirstObjectByType<Player>();
        _enemyMovement = GetComponent<EnemyMovement>();

        if (_player == null)
            Destroy(gameObject);

        StartSpawnSequence();

        _health = _maxHealth;
    }

    protected bool CanAttack() => _enemyRenderer.enabled; // Retorna True caso _enemyRender is True "Esta ativado"

    #region Spawn Sequence
    private void StartSpawnSequence()
    {
        SetEnemyRendererVisibility(false); // Disable _enemyRender and Enable _SpawnIndicator 

        // Scale up & Scale Down the Spawn Indicator using Tween Library
        // After 4 seconds Show the Enemy Renderer
        Vector3 targetScale = _spawnIndicatorRenderer.transform.localScale * 1.2f;
        LeanTween.scale(_spawnIndicatorRenderer.gameObject, targetScale, 0.3f)
            .setLoopPingPong(4) // Time in Loop .... After 4 seconds
            .setOnComplete(SpawnSequenceCompleted); // After 4 Seconds.... Call this method
    }
    private void SpawnSequenceCompleted()
    {
        SetEnemyRendererVisibility();
        _enemyCollider.enabled = true;
        _enemyMovement.StorePlayer(_player);

    }

    /// <summary>
    /// <para>TRUE: "enable _enemyRenderer" and "disable _spawnIndicatorRenderer".</para>
    /// <para>FALSE: "disable _enemyRenderer" and "enable _spawnIndicatorRenderer".</para>
    /// </summary>
    private void SetEnemyRendererVisibility(bool visibility = true)
    {
        _enemyRenderer.enabled = visibility;
        _spawnIndicatorRenderer.enabled = !visibility;
    }

    #endregion

    public void PassWay()
    {
        _particleSystem.transform.parent = null;
        _particleSystem.Play();
        Destroy(gameObject);

        OnEnemyPassWay?.Invoke(transform.position);
    }

    public void TakeDamage(int damage, bool isCriticalHit)
    {
        int realDamage = Mathf.Min(damage, _health);

        _health -= realDamage;

        if (_health <= 0)
            PassWay();

        OnDamageTaken?.Invoke(transform.position, realDamage, isCriticalHit);
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
