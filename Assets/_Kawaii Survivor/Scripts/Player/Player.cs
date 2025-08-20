using System;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth), typeof(PlayerLevel))]
public class Player : MonoBehaviour
{
    [Header("Elements")]
    public static Player instance;
    [SerializeField] private SpriteRenderer _characterRenderer;
    [SerializeField] private CircleCollider2D _collider;
    private PlayerHealth _playerHealth;
    private PlayerLevel _playerLevel;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        _playerHealth = GetComponent<PlayerHealth>();
        _playerLevel = GetComponent<PlayerLevel>();

        CharacterSelectionManager.onCharacterSelected += OnCharacterSelectedCallBack;
    }
    private void OnDestroy()
    {
        CharacterSelectionManager.onCharacterSelected -= OnCharacterSelectedCallBack;

    }

    public void TakeDamage(int damage)
    {
        _playerHealth.TakeDamage(damage);
    }

    public Vector2 GetCenter()
    {
        return (Vector2)transform.position + _collider.offset;
    }

    public bool HasLeveledUp() => _playerLevel.HasLeveledUp();


    // ACTIONS
    private void OnCharacterSelectedCallBack(CharacterDataSO characterData)
    {
        _characterRenderer.sprite = characterData.Sprite;
    }

}
