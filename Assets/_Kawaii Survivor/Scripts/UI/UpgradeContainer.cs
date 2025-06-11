using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _upgradeNameText;
    [SerializeField] private TextMeshProUGUI _upgradeValueText;

    [field: SerializeField] public Button Button { get; private set; }

    public void Configure(Sprite icon, string upgradeName, string upgradeValue)
    {
        _image.sprite = icon;
        _upgradeNameText.text = upgradeName;
        _upgradeValueText.text = upgradeValue;
    }
}
