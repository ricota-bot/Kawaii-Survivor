using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    [Header("DATA")]
    [SerializeField] private CharacterDataSO _characterData;

    [Header("Settings")]
    private Dictionary<Stat, float> playerStats = new Dictionary<Stat, float>();
    private Dictionary<Stat, float> addends = new Dictionary<Stat, float>();

    private void Awake()
    {
        playerStats = _characterData.BaseStats; // Inicializa o Dicionario com os Stats Base da Classe CharacterDataSO

        foreach (KeyValuePair<Stat, float> kvp in playerStats)
        {
            addends.Add(kvp.Key, 0);
        }
    }

    private void Start()
    {

        UpdatePlayerStats();
    }

    public void AddPlayerStat(Stat stat, float value)
    {

        if (addends.ContainsKey(stat))// Se contem aquele status
        {
            addends[stat] += value; // Incrementamos o valor do stats que o ja possui
        }
        else
        {
            Debug.Log($"The key {stat} has not been found.");
        }

        UpdatePlayerStats();
    }

    private void UpdatePlayerStats()
    {
        IEnumerable<IPlayerStatsDepedency> playerStatsDepedency = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IPlayerStatsDepedency>();

        foreach (IPlayerStatsDepedency dependency in playerStatsDepedency)
            dependency.UpdateStats(this);
    }

    public float GetStatValue(Stat stat)
    {
        float value = playerStats[stat] + addends[stat];
        return value;
    }
}
