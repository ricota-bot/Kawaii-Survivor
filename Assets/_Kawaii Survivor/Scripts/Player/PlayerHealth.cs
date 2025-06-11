using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class PlayerHealth : MonoBehaviour, IPlayerStatsDepedency
{
    [Header("Elements")]
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TextMeshProUGUI _healthText;

    [Header("Settings")]
    [SerializeField] private int _baseMaxHealth;
    private float _maxHealth;
    private float _health;

    private float _armor; // Se aumentamos o armor o player recebe menos dano .. basically is this :)
    private float _lifeStealPercent;
    private float _dodge;

    [Header("Life Recovery")]
    private float _healthRecoverySpeed;
    private float _healthRecoveryTimer;
    private float _healthRecoveryDuration;

    [Header("Actions")]
    public static Action<Vector2> OnAttackDodge;

    private void Awake()
    {
        Enemy.OnDamageTaken += EnemyOnDamageTakenCallBack;
    }

    private void OnDestroy()
    {
        Enemy.OnDamageTaken -= EnemyOnDamageTakenCallBack;

    }

    private void Update()
    {
        if (_health < _maxHealth)
        {
            RecoveryHealth();
        }

    }

    private void EnemyOnDamageTakenCallBack(Vector2 enemyPos, int damage, bool isCriticalHit)
    {
        // Se estamos com a vida cheia
        if (_health >= _maxHealth) // health is full
            return;

        float lifeStealValue = damage * _lifeStealPercent;

        float healthToAdd = Math.Min(lifeStealValue, _maxHealth - _health);

        Heal(healthToAdd);
        UpdateHealthUI();
    }

    private void RecoveryHealth()
    {
        _healthRecoveryTimer += Time.deltaTime;

        if (_healthRecoveryTimer >= _healthRecoveryDuration)
        {
            _healthRecoveryTimer = 0;
            float healthToAdd = Mathf.Min(0.1f, _maxHealth - _health);

            _health += healthToAdd;

            UpdateHealthUI();
        }
    }

    private void Heal(float amount)
    {
        _health += amount;
        // Particles Sound.. and another things you can Add...
    }

    public void TakeDamage(int damage)
    {
        if (ShouldDodge())
        {
            OnAttackDodge?.Invoke(transform.position);
            return;
        }

        float realDamage = damage * Mathf.Clamp(1 - (_armor / 1000), 0, 10000);
        realDamage = Mathf.Min(realDamage, _health); // Clamp between damageTaken and health, o dano não passa a vida maxima do player..

        _health -= realDamage;

        Debug.Log($"Real Damage {realDamage}");

        UpdateHealthUI();

        if (_health <= 0)
            PassAway();
    }

    private bool ShouldDodge()
    {
        return Random.Range(0f, 100f) < _dodge; // Is possibel to clamp dodge 
    }

    private void PassAway()
    {
        GameManager.instance.SetGameState(GameState.GAMEOVER);
    }

    private void UpdateHealthUI()
    {
        float healthBarValue = (float)_health / _maxHealth;
        _healthSlider.value = healthBarValue;
        _healthText.text = (int)_health + " / " + _maxHealth;
    }

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        float addedHealth = playerStatsManager.GetStatValue(Stat.MaxHealth);
        _maxHealth = _baseMaxHealth + (int)addedHealth;

        _maxHealth = Mathf.Max(_maxHealth, 1); // Utilizamos o 1 para que mantenha pelo menos 1 de acrescismo, caso pegamso um Downgrade que reduza a vida ao maximo

        _health = _maxHealth;

        UpdateHealthUI();

        _armor = playerStatsManager.GetStatValue(Stat.Armor);
        _lifeStealPercent = playerStatsManager.GetStatValue(Stat.LifeSteal) / 100; // Divide by 100 because we want it to be a percent...
        _dodge = playerStatsManager.GetStatValue(Stat.Dodge);

        _healthRecoverySpeed = Mathf.Max(.0001f, playerStatsManager.GetStatValue(Stat.HealthRecoverySpeed));

        _healthRecoveryDuration = 1f / _healthRecoverySpeed;
    }
}
