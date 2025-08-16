using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ESSE SCRIPT ESTA DENTRO DO DO OBJETO QUE FAZ PARA APARECER OS ITENS DENTRO DO INVENTARIO
// BASICAMENTO ESTAMOS CONFIGURANDO ESSE OBJETO FAZENDO APARECER O ICONE NOME.. COR DE FUNDO E ETC..
// ELE VAI SER CONFIGURADO COM OS ITENS: "WEAPON" E OS OBJETOS "OBJECTDATA"
public class InventoryItemInfo : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI recyclePriceText;

    [Header("Colors")]
    [SerializeField] private Image container;

    [Header("Stats")]
    [SerializeField] private Transform statsParent;

    [Header("Buttons")]
    [field: SerializeField] public Button RecycleButton { get; private set; }

    [SerializeField] private Button mergeButton;


    public void Configure(Weapon weapon)
    {
        Configure
            (weapon.WeaponData.Sprite,
            weapon.WeaponData.WeaponName + " (lvl " + (weapon.Level + 1) + ")",
            ColorHolder.GetColor(weapon.Level),
            WeaponStatsCalculator.GetRecyclePrice(weapon.WeaponData, weapon.Level),
            WeaponStatsCalculator.GetStats(weapon.WeaponData, weapon.Level)
            );

        mergeButton.gameObject.SetActive(WeaponMerge.instance.CanMerge(weapon));

        //mergeButton.interactable = WeaponMerge.instance.CanMerge(weapon); // This is a boolean

        mergeButton.onClick.RemoveAllListeners();
        mergeButton.onClick.AddListener(WeaponMerge.instance.Merge);


    }

    public void Configure(ObjectDataSO objectData)
    {
        Configure
            (objectData.Icon,
            objectData.Name,
            ColorHolder.GetColor(objectData.Rarity),
            objectData.RecyclePrice,
            objectData.BaseStats
            );
        mergeButton.gameObject.SetActive(false);

    }


    public void Configure(Sprite icon, string name, Color containerColor, int recyclePrice, Dictionary<Stat, float> stats)
    {
        this.icon.sprite = icon;
        itemNameText.text = name;
        itemNameText.color = containerColor;
        recyclePriceText.text = recyclePrice.ToString();

        container.color = containerColor;

        StatContainerManager.GenerateStatContainers(stats, statsParent);
    }
}
