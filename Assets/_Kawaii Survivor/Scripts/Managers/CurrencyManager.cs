using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [Header("Stuffs")]
    public static CurrencyManager Instance;

    [Header("Elements")]
    [field: SerializeField] public int Currency { get; private set; }

    [Header("Actions")]
    public static Action OnCurrencyChanged;

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

    [NaughtyAttributes.Button("Add 500 Currency")]
    public void Add500Currency() => AddCurrency(500);

    public void AddCurrency(int currency)
    {
        Currency += currency;
        UpdateText();
        OnCurrencyChanged?.Invoke();
    }


    private void UpdateText()
    {
        CurrencyText[] currencyText = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (CurrencyText text in currencyText)
        {
            text.UpdateText(Currency);
        }
    }

    public void UseCurrency(int currency)
    {
        if (CanAfford(currency))
            AddCurrency(-currency);
    }

    public bool CanAfford(int currency)
    {
        return Currency >= currency;
    }
}
