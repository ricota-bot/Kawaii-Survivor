using System.Collections.Generic;
using Tabsil.Sijil;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CharacterSelectionManager : MonoBehaviour, IWantToBeSaved
{
    [Header("References")]
    [SerializeField] private CharacterInfoContainer _characterInfoContainer;

    [Header("Elements")]
    [SerializeField] private Transform _characterButtonsParent;
    [SerializeField] private CharacterButton _characterButton;
    [SerializeField] private Image _characterCenterImage;

    [Header("Data")]
    private CharacterDataSO[] characterDatas;
    private List<bool> unlockedStatesList = new List<bool>();

    [Header("Constantes")]
    private const string _unlockedStatesKey = "UnlockedStatesKey";
    private const string _lastSelectedCharacterKey = "LastSelectedCharacterIndex";

    [Header("Settings")]
    private int selectedCharacterIndex;
    private int lastSelectedCharacterIndex;

    [Header("Action")]
    public static Action<CharacterDataSO> onCharacterSelected;

    private void Start()
    {
        _characterInfoContainer.Button.onClick.RemoveAllListeners();
        _characterInfoContainer.Button.onClick.AddListener(PurchaseSelectedCharacter);

        CharacterSelectedCallBack(lastSelectedCharacterIndex);
    }


    public void Initialize()
    {
        for (int i = 0; i < characterDatas.Length; i++)
        {
            CreateCharacterButton(i);
        }
    }

    private void CreateCharacterButton(int index) // Parte de Baixo o ScrollView com as fotinhas do personagem
    {

        CharacterDataSO characterData = characterDatas[index];

        CharacterButton characterButtonInstance = Instantiate(_characterButton, _characterButtonsParent);

        characterButtonInstance.Configure(characterData, unlockedStatesList[index]);

        characterButtonInstance.Button.onClick.RemoveAllListeners();
        characterButtonInstance.Button.onClick.AddListener(() => CharacterSelectedCallBack(index));
    }

    private void CharacterSelectedCallBack(int index)
    {
        // Armazenamos esse index para ser acessado de forma global :)
        selectedCharacterIndex = index; // Quando clicamos no botão do Scrol View "Selecionamos esse Index"

        CharacterDataSO characterData = characterDatas[index];

        if (unlockedStatesList[index]) // Caso na Lista de Desbloquedos estiver como true, desabilitamos o botão do container de comprar
        {
            _characterInfoContainer.Button.interactable = false;
            lastSelectedCharacterIndex = index;
            Save();
            onCharacterSelected?.Invoke(characterData);
        }
        else
        {
            _characterInfoContainer.Button.interactable =
                CurrencyManager.Instance.CanAffordPremiumCurrency(characterData.PurchasePrice);
        }

        _characterCenterImage.sprite = characterData.Sprite;
        _characterInfoContainer.Configure(characterData, unlockedStatesList[index]);
    }

    private void PurchaseSelectedCharacter()
    {
        int price = characterDatas[selectedCharacterIndex].PurchasePrice;
        CurrencyManager.Instance.UsePremiumCurrency(price);

        // Save the Unlocked State of tha character
        unlockedStatesList[selectedCharacterIndex] = true;

        // Update the Button Visuals "Is your Scroll Views Buttons Above"
        _characterButtonsParent.GetChild(selectedCharacterIndex).GetComponent<CharacterButton>().Unlock();

        // Update Character Info Panel "if your purchase" (HIDE PURCHASE CONTAINER PRICE)
        CharacterSelectedCallBack(selectedCharacterIndex); // Apenas chamamos o metodo que já faz isso :)

        Save();
    }

    public void Load()
    {
        characterDatas = ResourcesManager.Character;

        for (int i = 0; i < characterDatas.Length; i++)
        {
            // Quando Index for 0 ele da verdadeiro, marcando essa posição como true, no caso primeiro personagem desbloqueado :)
            unlockedStatesList.Add(i == 0); // Preenchemos todos os Character Data com exceto quando o index for "0" => retorna true
        }

        // LOAD UNLOCKEDS CHARACTER ("DESBLOQUEADOS")
        if (Sijil.TryLoad(this, _unlockedStatesKey, out object unlockedStatObject))
            unlockedStatesList = (List<bool>)unlockedStatObject;

        // LOAD LAST SELECTED CHARACTER
        if (Sijil.TryLoad(this, _lastSelectedCharacterKey, out object latSelectedCharacterObject))
            lastSelectedCharacterIndex = (int)latSelectedCharacterObject;

        Initialize();

        //CharacterSelectedCallBack(lastSelectedCharacterIndex);
    }

    public void Save()
    {
        Sijil.Save(this, _unlockedStatesKey, unlockedStatesList);
        Sijil.Save(this, _lastSelectedCharacterKey, lastSelectedCharacterIndex);
    }
}
