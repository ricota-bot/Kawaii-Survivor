using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshPro _damageText;


    [NaughtyAttributes.Button]
    public void PlayAnimation(int damage)
    {
        _animator.Play("DamageTextAnim");
        _damageText.text = damage.ToString();
    }
}
