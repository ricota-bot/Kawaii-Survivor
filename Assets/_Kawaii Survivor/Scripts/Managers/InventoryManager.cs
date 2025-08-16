using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameStateListener
{
    [Header("Player References")]
    [SerializeField] private PlayerObjects playerObjects;
    [SerializeField] private PlayerWeapons playerWeapons;

    [Header("Elements")]
    [SerializeField] private Transform inventoryItemContainerParent;
    [SerializeField] private InventoryItemContainer inventoryItemContainer;
    [SerializeField] private ShopManagerUI shopManagerUI;
    [SerializeField] private InventoryItemInfo inventoryItemInfo;

    private void Start()
    {
        ShopManager.onItemPurchased += OnPurchasedCallBack;
        WeaponMerge.onMerge += OnMergeCallBack;
    }

    private void OnDestroy()
    {
        ShopManager.onItemPurchased -= OnPurchasedCallBack;
        WeaponMerge.onMerge -= OnMergeCallBack;


    }

    private void OnMergeCallBack(Weapon weapon)
    {
        Configure();
        inventoryItemInfo.Configure(weapon);
    }

    private void OnPurchasedCallBack() => Configure();
    private void Configure()
    {
        inventoryItemContainerParent.Clear();

        // CONFIGURE WEAPONS
        Weapon[] weapons = playerWeapons.GetWeapons();

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null) continue;

            InventoryItemContainer containerInstance = Instantiate(inventoryItemContainer, inventoryItemContainerParent);
            containerInstance.Configure(weapons[i], i, () => ShowItensInformation(containerInstance));
        }
        // -----------------------------------------------------------------------------------------------------


        // CONFIGURE OBJECTS DATA
        ObjectDataSO[] objectsData = playerObjects.ObjectsData.ToArray();

        for (int i = 0; i < objectsData.Length; i++)
        {
            InventoryItemContainer containerInstance = Instantiate(inventoryItemContainer, inventoryItemContainerParent);
            containerInstance.Configure(objectsData[i], () => ShowItensInformation(containerInstance));
        }
        // -----------------------------------------------------------------------------------------------------
    }


    #region SHOW ITENS ON PANEL SLIDE INFORMATION
    private void ShowItensInformation(InventoryItemContainer container)
    {
        if (container.Weapon != null)
            ShowWeaponInfo(container.Weapon, container.Index);
        else
            ShowObjectInfo(container.ObjectData);
    }

    private void ShowWeaponInfo(Weapon weaponContainer, int weaponIndex)
    {
        inventoryItemInfo.Configure(weaponContainer);

        inventoryItemInfo.RecycleButton.onClick.RemoveAllListeners();
        inventoryItemInfo.RecycleButton.onClick.AddListener(() => RecycleWeapon(weaponIndex));


        shopManagerUI.ShowItemInfoPanel();
    }

    private void ShowObjectInfo(ObjectDataSO objectDataContainer)
    {
        inventoryItemInfo.Configure(objectDataContainer);

        inventoryItemInfo.RecycleButton.onClick.RemoveAllListeners();
        inventoryItemInfo.RecycleButton.onClick.AddListener(() => RecycleObject(objectDataContainer));

        shopManagerUI.ShowItemInfoPanel();
    }


    #endregion

    #region RECYCLING OBJECTS "BUTTON"
    private void RecycleObject(ObjectDataSO objectDataToRecycle)
    {
        // Remove Object from PlayerObject Script
        playerObjects.RecycleObject(objectDataToRecycle);

        // Reconfigure your inventory
        Configure();

        // Close item Info
        shopManagerUI.HideItemInfoPanel();
    }

    private void RecycleWeapon(int weaponIndex)
    {
        // Remove Weapon from PlayerWeapons Script (Para remover uma arma precisamos do seu Index)
        Debug.Log($"Recycle weapon at index {weaponIndex}");
        playerWeapons.RecycleWeapon(weaponIndex);

        // Reconfigure your inventory
        Configure();

        // Close item Info
        shopManagerUI.HideItemInfoPanel();
    }

    #endregion

    #region GAME STATE
    public void OnGameStateChangedCallBack(GameState state)
    {
        switch (state)
        {
            case GameState.SHOP:
                Configure();
                break;
        }
    }
    #endregion
}
