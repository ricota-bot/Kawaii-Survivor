using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class HopOnTap : MonoBehaviour, IPointerDownHandler
{
    [Header("Elements")]
    private RectTransform rectTransform;
    private Vector2 initialPosition;

    private void Awake() => rectTransform = GetComponent<RectTransform>();
    private void Start() => initialPosition = rectTransform.anchoredPosition;


    public void OnPointerDown(PointerEventData eventData)
    {
        float targetY = initialPosition.y + Screen.height / 50;

        LeanTween.cancel(gameObject);
        // Resetar a posi��o antes do novo punch
        rectTransform.anchoredPosition = initialPosition;

        LeanTween.moveY(rectTransform, targetY, .6f)
            .setEase(LeanTweenType.punch)
            .setIgnoreTimeScale(true);
    }
}
