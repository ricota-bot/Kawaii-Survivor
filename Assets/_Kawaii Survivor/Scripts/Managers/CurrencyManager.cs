using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [Header("Stuffs")]
    public static CurrencyManager Instance;

    [Header("Elements")]
    [field: SerializeField] public int Currency { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateText();
    }

    public void AddCurrency(int currencyAmount)
    {
        Currency += currencyAmount;
        UpdateText();
    }


    private void UpdateText()
    {
        CurrencyText[] currencyText = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (CurrencyText text in currencyText)
        {
            text.UpdateText(Currency);
        }
    }
}
