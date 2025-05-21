using UnityEngine;

public class DropManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Candy _candyPrefab;

    private void Awake()
    {
        Enemy.OnEnemyPassWay += OnEnemyPassWayCallBack;
    }

    private void OnDestroy()
    {
        Enemy.OnEnemyPassWay -= OnEnemyPassWayCallBack;

    }

    private void OnEnemyPassWayCallBack(Vector2 enemyPosition)
    {
        Instantiate(_candyPrefab, enemyPosition, Quaternion.identity,transform);
    }

}
