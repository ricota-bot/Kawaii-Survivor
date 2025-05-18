using UnityEngine;

public enum EnemyMovementType
{
    Direct,
    KeepDistance,
}
public class EnemyMovement : MonoBehaviour
{
    [Header("Elements")]
    private Player _player;

    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private EnemyMovementType _movementType;

    [Header("Extra Settings")]
    [SerializeField] private float _safeDistance;

    [Header("Display Distance Gizmos")]
    [SerializeField] private bool enableGizmos;

    private void Update()
    {
        if (_player != null)
            MoveEnemyToPlayer();
    }

    public void StorePlayer(Player player)
    {
        _player = player;
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
        }
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

    public void MoveKeepDistance()
    {
        Vector2 directionNormalized = (_player.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, _player.transform.position);

        if (distance > _safeDistance)
        {
            Vector2 move = directionNormalized * _moveSpeed * Time.deltaTime;
            transform.position = (Vector2)transform.position + move;
        }
    }

    #endregion


    #region GIZMOS
    private void OnDrawGizmos()
    {
        if (enableGizmos && _movementType == EnemyMovementType.KeepDistance)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, _safeDistance);
        }

    }

    #endregion
}
