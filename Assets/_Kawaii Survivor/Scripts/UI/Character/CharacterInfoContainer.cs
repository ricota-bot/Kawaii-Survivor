using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _purchasePrice;
    [SerializeField] private GameObject _purchasePriceContainer;
    [SerializeField] private Transform _statsParent;

    [Header("Button Component")]
    [field: SerializeField] public Button Button { get; private set; }

    public void Configure(CharacterDataSO characterData, bool unlocked)
    {
        _name.text = characterData.CharacterName;
        _purchasePrice.text = characterData.PurchasePrice.ToString();

        // Caso estiver blocked o botão vai ser ativado o contrario então fica true, assim podemos clickar nele
        _purchasePriceContainer.SetActive(!unlocked); //ex: Caso ele Estiver Desbloquado, vamos esconder o Botão então por isso é false

        StatContainerManager.GenerateStatContainers(characterData.NonNeutralStats, _statsParent);
    }
}
