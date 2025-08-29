using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Scriptable Objects/New Weapon Data", order = 0)]
public class WeaponDataSO : ScriptableObject
{
    [field: SerializeField] public string WeaponName { get; private set; }
    [ShowAssetPreview]
    [SerializeField] private Sprite _sprite;
    public Sprite Sprite { get => _sprite; private set => _sprite = value; }
    [field: SerializeField] public Weapon Prefab { get; private set; }
    [field: SerializeField] public int PurchasePrice { get; private set; }
    [field: SerializeField] public int RecyclePrice { get; private set; }

    [field: SerializeField] public AudioClip AttackSound { get; private set; } // Isto é um AudioClip :)

    [HorizontalLine(color: EColor.Yellow)]
    [SerializeField] private float attack;  // Percent
    [SerializeField] private float attackSpeed; // 0
    [SerializeField] private float criticalChance; //0
    [SerializeField] private float criticalPercent; //0
    [SerializeField] private float range;//0

    public Dictionary<Stat, float> BaseStats
    {
        get
        {
            return new Dictionary<Stat, float>()
            {
                {Stat.Attack, attack},
                {Stat.AttackSpeed, attackSpeed},
                {Stat.CriticalChance, criticalChance},
                {Stat.CriticalPercent, criticalPercent},
                {Stat.Range, range},

            };
        }

        private set { }
    }

    public float GetStatValue(Stat stat)
    {
        foreach (KeyValuePair<Stat, float> pair in BaseStats)
            if (pair.Key == stat)
                return pair.Value;

        Debug.LogError("Stat not Found.... ERROR this is not Normal");
        return 0;
    }
}
