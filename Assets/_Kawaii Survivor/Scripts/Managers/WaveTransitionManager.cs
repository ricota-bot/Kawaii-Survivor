using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

using Random = UnityEngine.Random;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private PlayerStatsManager playerStatsManager;

    [SerializeField] private UpgradeContainer[] upgradeContainers;


    public void OnGameStateChangedCallBack(GameState state)
    {
        switch (state)
        {
            case GameState.WAVETRANSITION:
                ConfigureUpgradeContainers();
                break;
        }
    }

    [Button]
    private void ConfigureUpgradeContainers()
    {
        for (int i = 0; i < upgradeContainers.Length; i++)
        {

            int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length); // Pegamos Posição
            Stat stat = (Stat)Enum.GetValues(typeof(Stat)).GetValue(randomIndex);

            string randomStatString = Enums.FormatStatName(stat);

            string buttonString;
            Action action = GetActionPerform(stat, out buttonString);


            upgradeContainers[i].Configure(null, randomStatString, buttonString);

            upgradeContainers[i].Button.onClick.RemoveAllListeners();
            upgradeContainers[i].Button.onClick.AddListener(() => action?.Invoke());
            upgradeContainers[i].Button.onClick.AddListener(() => BonusSelectedCallBack());
        }
    }


    private void BonusSelectedCallBack()
    {
        GameManager.instance.WaveCompleteCallBack();
    }

    private Action GetActionPerform(Stat stat, out string buttonString)
    {
        buttonString = "";
        float value;

        switch (stat)
        {
            case Stat.Attack:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";   // ex =>  5%
                break;

            case Stat.AttackSpeed:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";   // ex =>  5%
                break;

            case Stat.CriticalChance:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";   // ex =>  5%
                break;

            case Stat.CriticalPercent:
                value = Random.Range(1, 2);
                buttonString = "+" + value.ToString() + "x";   // ex =>  5%
                break;

            case Stat.MoveSpeed:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";   // ex =>  5%
                break;

            case Stat.MaxHealth:
                value = Random.Range(1, 10);
                buttonString = "+" + value;
                break;

            case Stat.Range:
                value = Random.Range(1, 5);
                buttonString = "+" + value.ToString();   // ex =>  5%
                break;

            case Stat.HealthRecoverySpeed:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";   // ex =>  5%
                break;

            case Stat.Armor:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";   // ex =>  5%
                break;

            case Stat.Luck:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";   // ex =>  5%
                break;

            case Stat.Dodge:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";   // ex =>  5%
                break;

            case Stat.LifeSteal:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";   // ex =>  5%
                break;

            default:
                return () => Debug.Log("Invalid Stat");

        }

        return () => playerStatsManager.AddPlayerStat(stat, value);
    }

}
