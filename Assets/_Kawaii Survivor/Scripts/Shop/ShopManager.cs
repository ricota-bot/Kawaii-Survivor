using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;



public class ShopManager : MonoBehaviour, IGameStateListener
{
    [Header("Player Components")]
    [SerializeField] private PlayerWeapons _playerWeapons;
    [SerializeField] private PlayerObjects _playerObjects;

    [Header("Elements")]
    [SerializeField] private Transform _shopContainersParent;
    [SerializeField] private ShopItemContainer _shopItemContainerPrefab;

    [Header("Reroll Button")]
    [SerializeField] private Button _rerollButton;
    [SerializeField] private int _rerollCost;
    [SerializeField] private TextMeshProUGUI _rerollCostText;

    [Header("Chests")]
    [SerializeField] private TextMeshProUGUI _chestAmountText;


    [Header("Actions")]
    public static Action onItemPurchased;

    private void Awake()
    {
        ShopItemContainer.OnPurchased += OnPurchasedCallBack;
        CurrencyManager.OnCurrencyChanged += OnCurrencyChangedCallBack;
    }

    private void OnDestroy()
    {
        ShopItemContainer.OnPurchased -= OnPurchasedCallBack;
        CurrencyManager.OnCurrencyChanged -= OnCurrencyChangedCallBack;

    }

    private void Configure()
    {
        List<GameObject> toDestroy = new List<GameObject>();

        for (int i = 0; i < _shopContainersParent.childCount; i++)
        {
            ShopItemContainer container = _shopContainersParent.GetChild(i).GetComponent<ShopItemContainer>();

            if (!container.IsLocked)
                toDestroy.Add(container.gameObject);
        }

        while (toDestroy.Count > 0)
        {
            GameObject containerToDestroy = toDestroy[0];
            containerToDestroy.transform.SetParent(null); // Unparent the container before destroying it
            toDestroy.RemoveAt(0);
            Destroy(containerToDestroy);
        }

        int containerToAdd = 6 - _shopContainersParent.childCount;
        int weaponContainerCount = Random.Range(Mathf.Min(2, containerToAdd), containerToAdd);
        int objectContainerCount = containerToAdd - weaponContainerCount;

        for (int i = 0; i < weaponContainerCount; i++)
        {
            var weaponContainerInstance = Instantiate(_shopItemContainerPrefab, _shopContainersParent);

            WeaponDataSO randomWeapon = ResourcesManager.GetRandomWeapon();
            weaponContainerInstance.Configure(randomWeapon, Random.Range(0, 2));


        }

        for (int i = 0; i < objectContainerCount; i++)
        {
            var objectContainerInstance = Instantiate(_shopItemContainerPrefab, _shopContainersParent);

            ObjectDataSO randomObject = ResourcesManager.GetRandomObject();

            objectContainerInstance.Configure(randomObject);

        }
    }

    public void RerollButtonCallBack()
    {
        Configure();
        CurrencyManager.Instance.UseCurrency(_rerollCost);

    }

    private void UpdateRerollVisuals()
    {
        _rerollCostText.text = _rerollCost.ToString();
        _rerollButton.interactable = CurrencyManager.Instance.CanAfford(_rerollCost);
    }


    // ACTIONS
    private void OnCurrencyChangedCallBack()
    {
        UpdateRerollVisuals();
    }

    private void OnPurchasedCallBack(ShopItemContainer container, int weaponLevel)
    {
        if (container.WeaponData != null) //
        {
            TryPurchaseWeapon(container, weaponLevel);
        }
        else
            PurchaseObject(container);

    }

    private void TryPurchaseWeapon(ShopItemContainer container, int weaponLevel)
    {
        if (_playerWeapons.TryAddWeapons(container.WeaponData, weaponLevel)) // If is True
        {
            // Quando comprar queremos remover o container
            container.transform.SetParent(null); // Unparent the container before destroying it
            Destroy(container.gameObject); // Destroy the container

            int currency = WeaponStatsCalculator.GetPurchasePrice(container.WeaponData, weaponLevel);
            CurrencyManager.Instance.UseCurrency(currency);

        }

        onItemPurchased?.Invoke();

    }

    private void PurchaseObject(ShopItemContainer container)
    {
        _playerObjects.AddObjects(container.ObjectData); // If is True

        // Quando comprar queremos remover o container
        container.transform.SetParent(null); // Unparent the container before destroying it
        Destroy(container.gameObject); // Destroy the container

        // Use the currency
        CurrencyManager.Instance.UseCurrency(container.ObjectData.Price);

        onItemPurchased?.Invoke();

    }

    public void OnGameStateChangedCallBack(GameState state)
    {
        switch (state)
        {
            case GameState.SHOP:
                Configure();
                UpdateRerollVisuals();
                break;
        }
    }
}
