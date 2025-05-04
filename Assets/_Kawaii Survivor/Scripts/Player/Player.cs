using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour
{
    [Header("Elements")]
    private PlayerHealth _playerHealth;
    private void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
    }

    public void TakeDamage(int damage)
    {
        _playerHealth.TakeDamage(damage);
    }

}
