using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image statImage;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statValueText;


    public void Configure(Sprite icon, string statName, float statValue, bool useColor = false)
    {
        statImage.sprite = icon;
        statNameText.text = statName;

        if (useColor)
            ColorizeStatValueText(statValue);
        else
        {
            statValueText.color = Color.white;
            statValueText.text = statValue.ToString("F2");
        }
    }

    private void ColorizeStatValueText(float statValue)
    {
        statValueText.color = Color.white;
        statValueText.text = statValue.ToString("F2");

        if (statValue > 0)
            statValueText.color = Color.green;
        else if (statValue < 0)
            statValueText.color = Color.red;


    }

    public float GetFontSize() => statNameText.fontSize;

    public void SetFontSize(float fontSize)
    {
        statNameText.fontSizeMax = fontSize;
        statValueText.fontSizeMax = fontSize;
    }
}
