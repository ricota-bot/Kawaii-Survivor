using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Objects/New Character Data", order = 0)]
public class CharacterDataSO : ScriptableObject
{
    [field: SerializeField] public string CharacterName { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public int PurchasePrice { get; private set; }

    [NaughtyAttributes.HorizontalLine]
    [SerializeField] private float attack;  // 5
    [SerializeField] private float attackSpeed; // 0
    [SerializeField] private float criticalChance; //0
    [SerializeField] private float criticalPercent; //0
    [SerializeField] private float moveSpeed;//0
    [SerializeField] private float maxHealth; // -1
    [SerializeField] private float range;//0
    [SerializeField] private float healthRecoverySpeed;//0
    [SerializeField] private float armor;//0
    [SerializeField] private float luck;//0
    [SerializeField] private float dodge;//0
    [SerializeField] private float lifeSteal;//0


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
                {Stat.MoveSpeed, moveSpeed},
                {Stat.MaxHealth, maxHealth},
                {Stat.Range, range},
                {Stat.HealthRecoverySpeed, healthRecoverySpeed},
                {Stat.Armor, armor},
                {Stat.Luck, luck},
                {Stat.Dodge, dodge},
                {Stat.LifeSteal, lifeSteal},

            };
        }

        private set { }
    }

    public Dictionary<Stat, float> NonNeutralStats
    {
        get
        {
            Dictionary<Stat, float> nonNeutralDictionary = new Dictionary<Stat, float>();

            foreach (KeyValuePair<Stat, float> pair in BaseStats)
            {
                if (pair.Value != 0)
                    nonNeutralDictionary.Add(pair.Key, pair.Value);
            }

            return nonNeutralDictionary;
        }
        private set { }
    }
}
