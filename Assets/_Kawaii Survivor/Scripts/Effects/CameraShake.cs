using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private float shakeMagnitude;
    [SerializeField] private float shakeDuration;

    private void Awake()
    {
        RangeWeapon.OnBulletShoot += Shake;
    }

    private void OnDestroy()
    {
        RangeWeapon.OnBulletShoot -= Shake;

    }

    private void Shake()
    {
        Vector2 direction = Random.onUnitSphere.With(z: 0).normalized;

        transform.localPosition = Vector2.zero;

        LeanTween.cancel(gameObject);
        LeanTween.moveLocal(gameObject, direction * shakeMagnitude, shakeDuration)
            .setEase(LeanTweenType.easeShake);
    }
}
