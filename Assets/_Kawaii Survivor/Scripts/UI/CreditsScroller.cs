using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CreditsScroller : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private float scrollSpeed;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        rectTransform.anchoredPosition = rectTransform.anchoredPosition.With(y: 0);
    }
    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition += Vector2.up * Time.deltaTime * scrollSpeed;
    }
}
