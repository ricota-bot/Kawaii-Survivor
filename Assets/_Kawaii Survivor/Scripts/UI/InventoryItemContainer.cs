using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Button button;
    [SerializeField] private Image container;
    [SerializeField] private Image icon;

    [Header("Propriedades")]
    public int Index { get; set; }
    public Weapon Weapon { get; private set; }
    public ObjectDataSO ObjectData { get; private set; }

    public void ConfigureInventoryItem(Color containerColor, Sprite itemIcon)
    {
        container.color = containerColor;
        this.icon.sprite = itemIcon;
    }

    public void Configure(Weapon weaponData, int index, Action onClickedCallBack)
    {
        Weapon = weaponData;
        Index = index;
        Color color = ColorHolder.GetColor(weaponData.Level);
        Sprite icon = weaponData.WeaponData.Sprite;

        ConfigureInventoryItem(color, icon);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClickedCallBack?.Invoke());
    }

    public void Configure(ObjectDataSO objectData, Action onClickedCallBack)
    {
        ObjectData = objectData;
        Color color = ColorHolder.GetColor(objectData.Rarity);
        Sprite icon = objectData.Icon;

        ConfigureInventoryItem(color, icon);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClickedCallBack?.Invoke());
    }
}
