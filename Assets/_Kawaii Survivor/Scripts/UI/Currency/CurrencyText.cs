using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CurrencyText : MonoBehaviour
{
    [Header("Elements")]
    TextMeshProUGUI _currencyText;

    public void UpdateText(int currencyValue)
    {
        if (_currencyText == null)
            _currencyText = GetComponent<TextMeshProUGUI>();

        int current = int.Parse(_currencyText.text);

        LeanTween.value(gameObject, current, currencyValue, 0.03f)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnUpdate((float val) =>
            {
                _currencyText.text = Mathf.RoundToInt(val).ToString();
            });
    }
}
