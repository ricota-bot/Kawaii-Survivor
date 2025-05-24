using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Pool;

public class DropManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Candy _candyPrefab;
    [SerializeField] private Cash _cashPrefab;
    [SerializeField] private Chest _chestPrefab;

    [Header("Settings")]
    [SerializeField][Range(0, 100)] private int _cashDropChance;
    [SerializeField][Range(0, 100)] private int _chestDropChance;

    [Header("Pooling")]
    private ObjectPool<Candy> _candyPool;
    private ObjectPool<Cash> _cashPool;


    private void Awake()
    {
        Enemy.OnEnemyPassWay += OnEnemyPassWayCallBack;
        Candy.OnDropCollected += ReleaseCandy;
        Cash.OnDropCollected += ReleaseCash;

    }

    private void OnDestroy()
    {
        Enemy.OnEnemyPassWay -= OnEnemyPassWayCallBack;
        Candy.OnDropCollected -= ReleaseCandy;
        Cash.OnDropCollected -= ReleaseCash;


    }

    private void Start()
    {
        //_candyPool = new ObjectPool<Candy>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy);
        _candyPool = new ObjectPool<Candy>(
            createFunc: () =>
            {
                var candy = Instantiate(_candyPrefab, transform);
                return candy;
            },
            actionOnGet: candy => candy.gameObject.SetActive(true),
            actionOnRelease: candy => candy.gameObject.SetActive(false),
            actionOnDestroy: candy => Destroy(candy));

        _cashPool = new ObjectPool<Cash>(
            createFunc: () =>
            {
                var cash = Instantiate(_cashPrefab, transform);
                return cash;
            },
            actionOnGet: cash => cash.gameObject.SetActive(true),
            actionOnRelease: cash => cash.gameObject.SetActive(false),
            actionOnDestroy: cash => Destroy(cash));

    }

    private void OnEnemyPassWayCallBack(Vector2 enemyPosition)
    {
        bool shouldSpawnCash = Random.Range(0, 101) <= _cashDropChance;  // 20% chance to....

        DroppableCurrency droppable = shouldSpawnCash ? _cashPool.Get() : _candyPool.Get();

        droppable.transform.position = enemyPosition;

        TryDropChest(enemyPosition);
    }

    private void TryDropChest(Vector2 enemyPosition)
    {
        bool shouldSpawnChest = Random.Range(0, 101) <= _chestDropChance;

        if (!shouldSpawnChest) // Caso não podemos Spawnar um Chest, apenas retornamos
            return;

        Instantiate(_chestPrefab, enemyPosition, Quaternion.identity, transform);
    }

    private void ReleaseCandy(Candy candy) =>
        _candyPool.Release(candy);

    private void ReleaseCash(Cash cash) =>
        _cashPool.Release(cash);


}
