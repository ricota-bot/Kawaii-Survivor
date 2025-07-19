using System;
using UnityEngine;

public class PlayerStatsDisplay : MonoBehaviour, IPlayerStatsDepedency
{
    [Header("Elements")]
    [SerializeField] private Transform _playerStatContainerParent;

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        int index = 0;

        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            StatContainer statContainer = _playerStatContainerParent.GetChild(index).GetComponent<StatContainer>();
            statContainer.gameObject.SetActive(true);

            Sprite icon = ResourcesManager.GetStatIcon(stat);
            string statName = Enums.FormatStatName(stat);
            float statValue = playerStatsManager.GetStatValue(stat);

            statContainer.Configure(icon, statName, statValue, true);

            index++;
        }

        for (int i = index; i < _playerStatContainerParent.childCount; i++)
            _playerStatContainerParent.GetChild(i).gameObject.SetActive(false);
    }

}
