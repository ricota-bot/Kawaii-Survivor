using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CharacterButton : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private GameObject lockButtonIcon;

    public Button Button
    {
        get { return GetComponent<Button>(); }
        private set { }
    }

    public void Configure(CharacterDataSO character, bool unlocked)
    {
        icon.sprite = character.Sprite;

        if (unlocked)
            Unlock();
        else
            Lock();
    }

    public void Unlock()
    {
        lockButtonIcon.SetActive(false);
        icon.color = Color.white;
    }

    public void Lock()
    {
        lockButtonIcon.SetActive(true);
        icon.color = Color.grey;
    }
}
