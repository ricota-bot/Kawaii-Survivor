using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    void Update()
    {
        _spriteRenderer.sortingOrder = -(int)(transform.position.y * 10);
    }
}
