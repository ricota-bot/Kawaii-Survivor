using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class WeaponContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _nameText;


    [Header("Stats")]
    [SerializeField] private Transform _statsContainerParent;

    [Header("Button")]
    [field: SerializeField] public Button Button { get; private set; }

    [Header("Color")]
    [SerializeField] private Image[] _backGroundContainers;

    public void Configure(Sprite icon, string name, int level, WeaponDataSO weaponData)
    {
        _icon.sprite = icon;
        _nameText.text = name + $" ({level + 1})";

        Color imageColor = ColorHolder.GetColor(level);

        _nameText.color = imageColor;

        foreach (var image in _backGroundContainers)
        {
            image.color = imageColor;
        }

        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainers(calculatedStats);
    }

    public void ConfigureStatContainers(Dictionary<Stat, float> calculatedStats)
    {
        StatContainerManager.GenerateStatContiners(calculatedStats, _statsContainerParent);
    }

    public void Select()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * 1.075f, 0.25f).setEase(LeanTweenType.easeInOutSine); // Increase Scale
    }
    public void Deselect()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one, 0.2f);

    }

}
