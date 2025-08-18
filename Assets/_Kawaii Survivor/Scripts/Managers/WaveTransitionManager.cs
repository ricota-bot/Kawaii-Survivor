using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

using Random = UnityEngine.Random;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    public static WaveTransitionManager Instance;
    [Header("Player")]
    [SerializeField] private PlayerObjects playerObjects;

    [Header("Elements")]
    [SerializeField] private PlayerStatsManager _playerStatsManager;
    [SerializeField] private UpgradeContainer[] _upgradeContainers;
    [SerializeField] private GameObject _upgradeContainersParent;

    [Header("Chest Related Stuffs")]
    [SerializeField] private ChestObjectContainer _chestObjectContainerPrefab;
    [SerializeField] private Transform _chestContainerParent;

    [SerializeField] private GameObject _chestCountContainer;
    private int _chestCollected;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Chest.OnCollected += OnChestCollectedCallBack;
    }

    private void OnDestroy()
    {
        Chest.OnCollected -= OnChestCollectedCallBack;

    }

    private void OnChestCollectedCallBack()
    {
        _chestCollected++;
        Debug.Log($"We now have {_chestCollected} chests");

        UpdateChestContainer(_chestCollected);
    }

    private void UpdateChestContainer(int chestCollected)
    {
        ChestCountText[] chestCountText = FindObjectsByType<ChestCountText>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (ChestCountText chestText in chestCountText)
            chestText.UpdateText(_chestCollected);
    }

    public bool HasCollectedChest() => _chestCollected > 0;

    public void OnGameStateChangedCallBack(GameState state)
    {
        switch (state)
        {
            case GameState.WAVETRANSITION:
                TryOpenChestContainer();
                break;
        }
    }

    private void TryOpenChestContainer()
    {
        _chestContainerParent.Clear();
        if (_chestCollected > 0)
            ShowChestContainer();
        else
            ConfigureUpgradeContainers();
    }

    private void ShowChestContainer()
    {
        _upgradeContainersParent.SetActive(false);
        _chestCountContainer.SetActive(true);

        UpdateChestContainer(_chestCollected);
        _chestCollected--;

        ObjectDataSO[] objectDatas = ResourcesManager.Objects;
        ObjectDataSO randomObjectData = objectDatas[Random.Range(0, objectDatas.Length)];

        ChestObjectContainer chestContainerInstance = Instantiate(_chestObjectContainerPrefab, _chestContainerParent);
        chestContainerInstance.Configure(randomObjectData);
        chestContainerInstance.TakeButton.onClick.AddListener(() => TakeButtonCallBack(randomObjectData));
        chestContainerInstance.RecycleButton.onClick.AddListener(() => RecycleButtonCallBack(randomObjectData));

    }

    private void TakeButtonCallBack(ObjectDataSO objectDataToTake)
    {
        playerObjects.AddObjects(objectDataToTake);
        TryOpenChestContainer();
    }

    private void RecycleButtonCallBack(ObjectDataSO objectToRecycle)
    {
        CurrencyManager.Instance.AddCurrency(objectToRecycle.RecyclePrice);
        TryOpenChestContainer();
    }

    [Button]
    private void ConfigureUpgradeContainers()
    {
        _upgradeContainersParent.SetActive(true);
        _chestCountContainer.SetActive(false);

        for (int i = 0; i < _upgradeContainers.Length; i++)
        {

            int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length); // Pegamos Posição
            Stat stat = (Stat)Enum.GetValues(typeof(Stat)).GetValue(randomIndex);

            Sprite upgradeSprite = ResourcesManager.GetStatIcon(stat);
            string randomStatString = Enums.FormatStatName(stat);

            string buttonString;
            Action action = GetActionPerform(stat, out buttonString);


            _upgradeContainers[i].Configure(upgradeSprite, randomStatString, buttonString);

            _upgradeContainers[i].Button.onClick.RemoveAllListeners();
            _upgradeContainers[i].Button.onClick.AddListener(() => action?.Invoke());
            _upgradeContainers[i].Button.onClick.AddListener(() => BonusSelectedCallBack());
        }
    }


    private void BonusSelectedCallBack()
    {
        GameManager.instance.WaveCompleteCallBack();
    }

    private Action GetActionPerform(Stat stat, out string buttonString)
    {
        buttonString = "";
        float value;

        switch (stat)
        {
            case Stat.Attack:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";   // ex =>  5%
                break;

            case Stat.AttackSpeed:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";   // ex =>  5%
                break;

            case Stat.CriticalChance:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";   // ex =>  5%
                break;

            case Stat.CriticalPercent:
                value = Random.Range(1, 2);
                buttonString = "+" + value.ToString() + "x";   // ex =>  5%
                break;

            case Stat.MoveSpeed:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";   // ex =>  5%
                break;

            case Stat.MaxHealth:
                value = Random.Range(1, 10);
                buttonString = "+" + value;
                break;

            case Stat.Range:
                value = Random.Range(1, 5);
                buttonString = "+" + value.ToString();   // ex =>  5%
                break;

            case Stat.HealthRecoverySpeed:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";   // ex =>  5%
                break;

            case Stat.Armor:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";   // ex =>  5%
                break;

            case Stat.Luck:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";   // ex =>  5%
                break;

            case Stat.Dodge:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";   // ex =>  5%
                break;

            case Stat.LifeSteal:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";   // ex =>  5%
                break;

            default:
                return () => Debug.Log("Invalid Stat");

        }
        //buttonString = Enums.FormatStatName(stat) + "\n" + buttonString;

        return () => _playerStatsManager.AddPlayerStat(stat, value);
    }

}
