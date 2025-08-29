using System;
using UnityEngine;
using UnityEngine.Pool;

public class DamageTextManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private DamageText _damageTextPrefab;

    [Header("Pooling")]
    private ObjectPool<DamageText> _damageTextPool;

    private void Awake()
    {
        Enemy.OnDamageTaken += OnDamageTakenCallBack;
        PlayerHealth.OnAttackDodge += OnAttackDodgeCallBack;
    }

    private void OnDestroy()
    {
        Enemy.OnDamageTaken -= OnDamageTakenCallBack;
        PlayerHealth.OnAttackDodge -= OnAttackDodgeCallBack;


    }

    private void Start()
    {
        _damageTextPool = new ObjectPool<DamageText>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    private DamageText CreateFunction()
    {
        return Instantiate(_damageTextPrefab, transform);
    }

    private void ActionOnGet(DamageText damageText)
    {
        damageText.gameObject.SetActive(true);
    }
    private void ActionOnRelease(DamageText damageText)
    {
        if (damageText != null) // Caso não estiver vazio a gente tenta desativar
            damageText.gameObject.SetActive(false);

    }

    private void ActionOnDestroy(DamageText damageText)
    {
        Destroy(damageText.gameObject);
    }

    private void OnDamageTakenCallBack(Vector2 enemyPosition, int damage, bool isCriticalHit)
    {
        var damageTextInstance = _damageTextPool.Get();

        Vector3 spawnPosition = enemyPosition + Vector2.up * 1;
        damageTextInstance.transform.position = spawnPosition;

        damageTextInstance.PlayAnimation(damage.ToString("F2"), isCriticalHit);

        LeanTween.delayedCall(1, () => _damageTextPool.Release(damageTextInstance));
    }


    private void OnAttackDodgeCallBack(Vector2 playerPosition)
    {
        var damageTextInstance = _damageTextPool.Get();

        Vector3 spawnPosition = playerPosition + Vector2.up * 1;
        damageTextInstance.transform.position = spawnPosition;

        damageTextInstance.PlayAnimation("Dodge", false);

        LeanTween.delayedCall(1, () => _damageTextPool.Release(damageTextInstance));
    }
}
