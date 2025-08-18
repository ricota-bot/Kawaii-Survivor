using System;
using UnityEngine;
using Tabsil.Sijil;

public class CurrencyManager : MonoBehaviour, IWantToBeSaved
{
    [Header("Stuffs")]
    public static CurrencyManager Instance;

    private const string _premiumCurrencyKey = "PremiumCurrency";

    [Header("Elements")]
    [field: SerializeField] public int Currency { get; private set; }
    [field: SerializeField] public int PremiumCurrency { get; private set; }
    [field: SerializeField] public int ChestCount { get; private set; }

    [Header("Actions")]
    public static Action OnCurrencyChanged;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // ACTIONS
        Candy.OnDropCollected += OnCandyCollectedCallBack;
        Cash.OnDropCollected += OnCashCollectedCallBack;
    }

    private void OnDestroy()
    {
        // ACTIONS
        Candy.OnDropCollected -= OnCandyCollectedCallBack;
        Cash.OnDropCollected -= OnCashCollectedCallBack;

    }

    private void Start()
    {
        UpdateText();
    }


    public void Load()
    {
        if (Sijil.TryLoad(this, _premiumCurrencyKey, out object premiumCurrencyValue))
            AddPremiumCurrency((int)premiumCurrencyValue, false); // Caso já temos valor nessa Key "_premiumCurrencyKey"
        else
            AddPremiumCurrency(100, false); // Caso não tenha nenhum valor na chave "_premiumCurrencyKey"

    }

    public void Save()
    {
        Sijil.Save(this, _premiumCurrencyKey, PremiumCurrency);
    }


    [NaughtyAttributes.Button("Add 500 Currency")]
    public void Add500Currency() => AddCurrency(500);

    [NaughtyAttributes.Button("Add 500 Premium Currency")]
    public void Add500PremiumCurrency() => AddPremiumCurrency(500);


    public void AddCurrency(int currency)
    {
        Currency += currency;
        UpdateVisuals();
    }

    public void AddPremiumCurrency(int currency, bool save = true)
    {
        PremiumCurrency += currency;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        UpdateText();
        OnCurrencyChanged?.Invoke();

        Save();
    }
    public void UseCurrency(int currency)
    {
        if (CanAfford(currency))
            AddCurrency(-currency);
    }
    public void UsePremiumCurrency(int currency)
    {
        if (CanAffordPremiumCurrency(currency))
            AddPremiumCurrency(-currency);
    }
    public bool CanAfford(int currency) => Currency >= currency;
    public bool CanAffordPremiumCurrency(int currency) => PremiumCurrency >= currency;

    // ACTIONS
    private void OnCandyCollectedCallBack(Candy candy) => AddCurrency(1);
    private void OnCashCollectedCallBack(Cash cash) => AddPremiumCurrency(1);



    public static void UpdateTextsCurrency() => Instance.UpdateText();
    private void UpdateText()
    {
        CurrencyText[] currencyText = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        PremiumCurrencyText[] premiumCurrencyText = FindObjectsByType<PremiumCurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (CurrencyText text in currencyText)
            text.UpdateText(Currency);

        foreach (PremiumCurrencyText premiumCurrency in premiumCurrencyText)
            premiumCurrency.UpdateText(PremiumCurrency);

    }
}
