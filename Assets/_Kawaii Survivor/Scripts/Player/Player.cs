using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private CircleCollider2D _collider;
    private PlayerHealth _playerHealth;
    private void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
    }

    public void TakeDamage(int damage)
    {
        _playerHealth.TakeDamage(damage);
    }

    public Vector2 GetCenter()
    {
        return (Vector2)transform.position + _collider.offset;
    }
}
