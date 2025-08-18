using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerDetection : MonoBehaviour
{
    [Header("Colliders")]
    [SerializeField] private Collider2D _playerCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ICollectable collectable))
        {
            if (!collision.IsTouching(_playerCollider))
                return;

            collectable.Collect(GetComponent<Player>());
        }
    }
}
