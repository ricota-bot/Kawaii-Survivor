using TMPro;
using UnityEngine;

public class ChestCountText : MonoBehaviour
{
    [Header("Elements")]
    TextMeshProUGUI _currencyText;

    public void UpdateText(int currencyValue)
    {
        if (_currencyText == null)
            _currencyText = GetComponent<TextMeshProUGUI>();

        _currencyText.text = currencyValue.ToString();
    }
}
