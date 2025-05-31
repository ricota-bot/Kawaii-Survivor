using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TextMeshProUGUI _healthText;

    [Header("Settings")]
    [SerializeField] private int _maxHealth;
    private int _health;

    private void Start()
    {
        _health = _maxHealth;

        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, _health);

        _health -= realDamage;

        UpdateHealthUI();

        if (_health <= 0)
            PassAway();

    }

    private void PassAway()
    {
        GameManager.instance.SetGameState(GameState.GAMEOVER);
    }

    private void UpdateHealthUI()
    {
        float healthBarValue = (float)_health / _maxHealth;
        _healthSlider.value = healthBarValue;
        _healthText.text = _health + " / " + _maxHealth;
    }
}
