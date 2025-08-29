using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class BumpyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("Elements")]
    private Button button;

    [Header("Action")]
    public static Action OnButtonPressed;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!button.interactable) // Caso o botão não seja interactable apenas retornamos 
            return;

        LeanTween.cancel(button.gameObject);
        LeanTween.scale(gameObject, new Vector2(1.1f, 0.9f), 0.5f)
            .setEase(LeanTweenType.easeOutElastic)
            .setIgnoreTimeScale(true);


    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!button.interactable) // Caso o botão não seja interactable apenas retornamos 
            return;

        LeanTween.cancel(button.gameObject);
        LeanTween.scale(gameObject, Vector2.one, 0.5f)
            .setEase(LeanTweenType.easeOutElastic)
            .setIgnoreTimeScale(true);

        OnButtonPressed?.Invoke();

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable) // Caso o botão não seja interactable apenas retornamos 
            return;

        LeanTween.cancel(button.gameObject);
        LeanTween.scale(gameObject, Vector2.one, 0.5f)
            .setEase(LeanTweenType.easeOutElastic)
            .setIgnoreTimeScale(true);
    }
}
