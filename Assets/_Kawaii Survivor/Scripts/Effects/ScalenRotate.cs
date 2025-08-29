using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ScalenRotate : MonoBehaviour, IPointerDownHandler
{
    [Header("Elements")]
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);

        transform.localScale = Vector3.one; // Reset LocalScale
        LeanTween.scale(rectTransform, Vector2.one * 1.1f, 1f)
            .setEase(LeanTweenType.punch)
            .setIgnoreTimeScale(true);

        rectTransform.rotation = Quaternion.identity;
        int sign = (int)Mathf.Sign(Random.Range(-1, 1)); // Retorna -1 e 0

        LeanTween.rotateAround(rectTransform, Vector3.forward, 15 * sign, 1)
            .setEase(LeanTween.punch)
            .setIgnoreTimeScale(true);

    }

}
