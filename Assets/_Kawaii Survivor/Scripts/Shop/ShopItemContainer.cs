using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _priceText;

    [Header("Stats")]
    [SerializeField] private Transform _statsContainerParent;

    [Header("Button")]
    [SerializeField] public Button purchaseButton;

    [Header("Color")]
    [SerializeField] private Image[] _backGroundContainers;
    [SerializeField] private Image _outlineContainer;

    [Header("Lock Elements")]
    [SerializeField] private Image _lockButtonImage;
    [SerializeField] private Sprite _lockedSprite, _unlockedSprite;
    public bool IsLocked { get; private set; }

    [Header("Purchasing")]
    public WeaponDataSO WeaponData { get; private set; }
    public ObjectDataSO ObjectData { get; private set; }

    private int weaponLevel;
    private int itemPrice;

    [Header("Actions")]
    public static Action<ShopItemContainer, int> OnPurchased;

    private void Awake()
    {
        CurrencyManager.OnCurrencyChanged += OnCurrencyChangedCallBack;
    }

    private void OnDestroy()
    {
        CurrencyManager.OnCurrencyChanged -= OnCurrencyChangedCallBack;
    }

    // ACTIONS
    private void OnCurrencyChangedCallBack()
    {

        if (WeaponData != null)
            itemPrice = WeaponStatsCalculator.GetPurchasePrice(WeaponData, weaponLevel);
        else
            itemPrice = ObjectData.Price;

        purchaseButton.interactable = CurrencyManager.Instance.CanAfford(itemPrice);
    }


    public void Configure(WeaponDataSO weaponData, int level)
    {
        WeaponData = weaponData;
        weaponLevel = level;
        _icon.sprite = weaponData.Sprite;
        _nameText.text = weaponData.WeaponName;
        //_nameText.text = weaponData.WeaponName + $" ({level + 1})";

        int weaponPrice = WeaponStatsCalculator.GetPurchasePrice(weaponData, level);
        _priceText.text = weaponPrice.ToString();

        Color imageColor = ColorHolder.GetColor(level);

        _nameText.color = imageColor;
        _outlineContainer.color = ColorHolder.GetOutlineColor(level);

        foreach (var image in _backGroundContainers)
        {
            image.color = imageColor;
        }


        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainers(calculatedStats);

        purchaseButton.onClick.AddListener(Purchase);
        purchaseButton.interactable = CurrencyManager.Instance.CanAfford(weaponPrice);
    }

    public void Configure(ObjectDataSO objectData)
    {
        ObjectData = objectData;
        _icon.sprite = objectData.Icon;
        _nameText.text = objectData.Name;
        _priceText.text = objectData.Price.ToString();

        Color imageColor = ColorHolder.GetColor(objectData.Rarity);

        _nameText.color = imageColor;
        _outlineContainer.color = ColorHolder.GetOutlineColor(objectData.Rarity);

        foreach (var image in _backGroundContainers)
        {
            image.color = imageColor;
        }

        ConfigureStatContainers(objectData.BaseStats);

        purchaseButton.onClick.AddListener(Purchase);
        purchaseButton.interactable = CurrencyManager.Instance.CanAfford(objectData.Price);

    }

    public void ConfigureStatContainers(Dictionary<Stat, float> stats)
    {
        _statsContainerParent.Clear();
        StatContainerManager.GenerateStatContainers(stats, _statsContainerParent);
    }

    public void LockButtonCallBack()
    {
        IsLocked = !IsLocked;
        UpdateLockVisuals();
    }

    private void UpdateLockVisuals()
    {
        _lockButtonImage.sprite = IsLocked ? _lockedSprite : _unlockedSprite;
        purchaseButton.interactable = !IsLocked;

        if (!CurrencyManager.Instance.CanAfford(itemPrice))
        {

            purchaseButton.interactable = false;
        }

    }

    private void Purchase()
    {
        OnPurchased?.Invoke(this, weaponLevel);
    }

}
