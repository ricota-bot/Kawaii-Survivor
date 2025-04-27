using UnityEngine;

public enum EnemyMovementType
{
    Direct,
    KeepDistance,
    Wobble,
    Curve,
    SuperExpressive
}
public class EnemyMovement : MonoBehaviour
{
    [Header("Elements")]
    private Player _player;

    [Header("Spawn Sequence Related")]
    [SerializeField] private SpriteRenderer _enemyRenderer;
    [SerializeField] private SpriteRenderer _spawnIndicatorRenderer;
    private bool _hasSpawned;

    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private EnemyMovementType _movementType;

    [Header("Extra Settings")]
    [SerializeField] private float _safeDistance = 2f;
    [SerializeField] private float _curveIntensity = 0.5f;
    [SerializeField] private float _wobbleIntensity = 0.3f;
    [SerializeField] private float _wobbleSpeed = 1.5f;

    [Header("Detection Radius")]
    [SerializeField] private float playerDetectRadius;

    [Header("Debug")]
    [SerializeField] private bool displayGizmos;

    [Header("Particles")]
    [SerializeField] private ParticleSystem _particleSystem;




    private void Start()
    {
        _player = FindFirstObjectByType<Player>();

        if (_player == null)
            Destroy(gameObject);

        // Hide Enemy Renderer
        _enemyRenderer.enabled = false;
        // Show the Spawn Indicator  (Prevent Following & Attacking during the Spawn Sequence) Velocity equals ZERO ?
        _spawnIndicatorRenderer.enabled = true;

        // Scale up & Scale Down the Spawn Indicator using Tween Library
        // After 4 seconds Show the Enemy Renderer
        Vector3 targetScale = _spawnIndicatorRenderer.transform.localScale * 1.2f;
        LeanTween.scale(_spawnIndicatorRenderer.gameObject, targetScale, 0.3f)
            .setLoopPingPong(4)
            .setOnComplete(SpawnSequenceCompleted);

        // Hide The Spawn Indicator
    }

    private void Update()
    {
        if (!_hasSpawned) // Se não foi Spawnado o Enemy, então apenas retornamos "hasSpawned == false"
            return;

        MoveEnemyToPlayer();
        EnemyNearlyToPlayer();
    }

    private void SpawnSequenceCompleted()
    {
        // Disable SpawnIndicator
        _spawnIndicatorRenderer.enabled = false;
        // Enable Enemy Renderer
        _enemyRenderer.enabled = true;
        // Set Spawned to True to move the Enemy Again
        _hasSpawned = true;
    }

    private void MoveEnemyToPlayer()
    {
        if (_player == null)
            return;

        switch (_movementType)
        {
            case EnemyMovementType.Direct:
                MoveDirect();
                break;

            case EnemyMovementType.KeepDistance:
                MoveKeepDistance();
                break;

            case EnemyMovementType.Wobble:
                MoveWithWobble();
                break;

            case EnemyMovementType.Curve:
                MoveWithCurve();
                break;

            case EnemyMovementType.SuperExpressive:
                MoveSuperExpressive();
                break;
        }
    }

    private void EnemyNearlyToPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);

        if (distanceToPlayer <= playerDetectRadius)
            TryAttack();
    }

    private void TryAttack()
    {
        Debug.Log("Atack Atack Atack..... bAW BAW BAW");
        EnemyDeath();
    }

    private void EnemyDeath()
    {
        _particleSystem.transform.parent = null;
        _particleSystem.Play();
        Destroy(gameObject);
    }

    #region Different Movements Paths
    private void MoveDirect()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            _player.transform.position,
            _moveSpeed * Time.deltaTime
        );
    }

    private void MoveKeepDistance()
    {
        Vector2 directionNormalized = (_player.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, _player.transform.position);

        if (distance > _safeDistance)
        {
            Vector2 move = directionNormalized * _moveSpeed * Time.deltaTime;
            transform.position = (Vector2)transform.position + move;
        }
    }

    private void MoveWithWobble()
    {
        Vector2 directionToPlayer = (_player.transform.position - transform.position).normalized;

        Vector2 randomWobble = new Vector2(
            Mathf.PerlinNoise(Time.time * _wobbleSpeed, 0f) - 0.5f,
            Mathf.PerlinNoise(0f, Time.time * _wobbleSpeed) - 0.5f
        );

        Vector2 finalDirection = (directionToPlayer + randomWobble * _wobbleIntensity).normalized;

        Vector2 move = finalDirection * _moveSpeed * Time.deltaTime;
        transform.position = (Vector2)transform.position + move;
    }

    private void MoveWithCurve()
    {
        Vector2 directionToPlayer = (_player.transform.position - transform.position).normalized;

        Vector2 perpendicular = new Vector2(-directionToPlayer.y, directionToPlayer.x);

        Vector2 curveDirection = (directionToPlayer + perpendicular * _curveIntensity).normalized;

        Vector2 move = curveDirection * _moveSpeed * Time.deltaTime;
        transform.position = (Vector2)transform.position + move;
    }

    private void MoveSuperExpressive()
    {
        Vector2 directionToPlayer = (_player.transform.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);

        float speedMultiplier = Mathf.Clamp(distanceToPlayer / 3f, 0.5f, 3f);

        Vector2 perpendicular = new Vector2(-directionToPlayer.y, directionToPlayer.x);
        Vector2 curveDirection = (directionToPlayer + perpendicular * _curveIntensity).normalized;

        Vector2 randomWobble = new Vector2(
            Mathf.PerlinNoise(Time.time * _wobbleSpeed, 0f) - 0.5f,
            Mathf.PerlinNoise(0f, Time.time * _wobbleSpeed) - 0.5f
        );

        Vector2 finalDirection = (curveDirection + randomWobble * _wobbleIntensity).normalized;

        Vector2 move = finalDirection * _moveSpeed * speedMultiplier * Time.deltaTime;
        transform.position = (Vector2)transform.position + move;
    }

    #endregion


    private void OnDrawGizmos()
    {
        if (!displayGizmos)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, playerDetectRadius);
    }
}
