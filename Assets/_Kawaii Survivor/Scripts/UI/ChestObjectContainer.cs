using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestObjectContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _recyclePriceText;

    [Header("Stats")]
    [SerializeField] private Transform _statsContainerParent;

    [Header("Button")]
    [field: SerializeField] public Button TakeButton { get; private set; }
    [field: SerializeField] public Button RecycleButton { get; private set; }

    [Header("Color")]
    [SerializeField] private Image[] _backGroundContainers;
    [SerializeField] private Image _outlineContainer;


    public void Configure(ObjectDataSO objectData)
    {
        _icon.sprite = objectData.Icon;
        _nameText.text = objectData.Name;
        _recyclePriceText.text = objectData.RecyclePrice.ToString();

        Color imageColor = ColorHolder.GetColor(objectData.Rarity);

        _nameText.color = imageColor;
        _outlineContainer.color = ColorHolder.GetOutlineColor(objectData.Rarity);

        foreach (var image in _backGroundContainers)
        {
            image.color = imageColor;
        }


        ConfigureStatContainers(objectData.BaseStats);
    }

    public void ConfigureStatContainers(Dictionary<Stat, float> stats)
    {
        StatContainerManager.GenerateStatContainers(stats, _statsContainerParent);
    }

}
