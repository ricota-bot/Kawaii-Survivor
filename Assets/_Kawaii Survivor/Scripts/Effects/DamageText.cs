using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshPro _damageText;


    [NaughtyAttributes.Button]
    public void PlayAnimation(string damage, bool isCriticalHit)
    {
        _damageText.color = isCriticalHit ? Color.yellow : Color.white;
        _damageText.text = damage.ToString();

        _animator.Play("DamageTextAnim");
    }
}
