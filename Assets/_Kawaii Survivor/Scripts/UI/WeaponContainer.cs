using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _nameText;

    [field: SerializeField] public Button Button { get; private set; }

    [Header("Color")]
    [SerializeField] private Image[] _backGroundContainers;

    private Color _originalColor;

    public void Configure(Sprite icon, string name, int level)
    {
        _icon.sprite = icon;
        _nameText.text = name;

        Color imageColor = ColorHolder.GetColor(level);

        foreach (var image in _backGroundContainers)
        {
            image.color = imageColor;
        }
    }

    public void Select()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * 1.075f, 0.25f).setEase(LeanTweenType.easeInOutSine); // Increase Scale
    }

    public void Deselect()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one, 0.2f);

    }


    private void ApplyHSVValueChange(float valueMultiplier)
    {
        Color.RGBToHSV(_originalColor, out float H, out float S, out float V);

        V *= valueMultiplier;
        V = Mathf.Clamp01(V); // Garante que fica entre 0 e 1

        Color newColor = Color.HSVToRGB(H, S, V);
        newColor.a = _originalColor.a; // Mantém a transparência

        //_backGroundContainers.color = newColor;
    }

}
