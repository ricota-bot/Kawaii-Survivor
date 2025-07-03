using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image statImage;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statValueText;


    public void Configure(Sprite icon, string statName, string statValue)
    {
        statImage.sprite = icon;
        statNameText.text = statName;
        statValueText.text = statValue;
    }

    public float GetFontSize() => statNameText.fontSize;

    public void SetFontSize(float fontSize)
    {
        statNameText.fontSizeMax = fontSize;
        statValueText.fontSizeMax = fontSize;
    }
}
