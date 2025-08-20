using System;
using Tabsil.Sijil;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour, IWantToBeSaved
{
    [Header("Settings")]
    [SerializeField] private Button sfxButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button privacyPolicyButton;
    [SerializeField] private Button askButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private GameObject creditsPanel;

    [Header("Elements")]
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;

    [Header("Actions")]
    public static Action<bool> OnSFXStateChanged;
    public static Action<bool> OnMusicStateChanged;

    private bool sfxState;
    private bool musicState;

    private const string sfxStateKey = "sfxStateKey";
    private const string musicStateKey = "musicStateKey";

    private void Awake()
    {
        // SFX
        sfxButton.onClick.RemoveAllListeners();
        sfxButton.onClick.AddListener(SFXButtonCallBack);

        // MUSIC
        musicButton.onClick.RemoveAllListeners();
        musicButton.onClick.AddListener(MusicButtonCallBack);

        // PRIVACY POLICY BUTTON
        privacyPolicyButton.onClick.RemoveAllListeners();
        privacyPolicyButton.onClick.AddListener(PrivacyPolicyButtonCallBack);

        // HELP E SUPPORT "ASK"
        askButton.onClick.RemoveAllListeners();
        askButton.onClick.AddListener(AskButtonCallBack);

        creditsButton.onClick.RemoveAllListeners();
        creditsButton.onClick.AddListener(CreditsButtonCallBack);

    }


    private void Start()
    {
        HideCreditsPanel();
    }

    // SFX
    private void SFXButtonCallBack()
    {
        sfxState = !sfxState;
        UpdateButtonVisuals(sfxButton, sfxState);

        Save();
        // Trigger an "ACTION" with the new SFX state.. to mute sound in SoundManager Class... "Something like this :)"
        OnSFXStateChanged?.Invoke(sfxState);
    }

    // MUSIC
    private void MusicButtonCallBack()
    {
        musicState = !musicState;
        UpdateButtonVisuals(musicButton, musicState);

        Save();
        // Trigger an "ACTION" with the new SFX state.. to mute sound in SoundManager Class... "Something like this :)"
        OnMusicStateChanged?.Invoke(musicState);
    }

    private void PrivacyPolicyButtonCallBack()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=cfGdhbCJgnE&ab_channel=Hollts");
    }

    private void AskButtonCallBack()
    {
        string email = "pehenguee@gmail.com";
        string subject = MyEscapeURL("Help");

        string body = MyEscapeURL("Hey !! I need help with this ...");

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    private string MyEscapeURL(string s)
    {
        return UnityWebRequest.EscapeURL(s).Replace("+", "%20");
    }

    private void CreditsButtonCallBack()
    {
        creditsPanel.SetActive(true);
    }

    public void HideCreditsPanel()
    {
        creditsPanel.SetActive(false);
    }


    private void UpdateButtonVisuals(Button button, bool state)
    {
        if (state)
        {
            button.image.color = onColor;
            button.GetComponentInChildren<TextMeshProUGUI>().text = "ON";
        }
        else
        {
            button.image.color = offColor;
            button.GetComponentInChildren<TextMeshProUGUI>().text = "OFF";
        }
    }

    public void Load()
    {
        sfxState = true;
        musicState = true;

        if (Sijil.TryLoad(this, sfxStateKey, out object sfxStateValue))
        {
            sfxState = (bool)sfxStateValue;
        }

        if (Sijil.TryLoad(this, musicStateKey, out object musicStateValue))
        {
            musicState = (bool)musicStateValue;
        }

        UpdateButtonVisuals(sfxButton, sfxState);
        UpdateButtonVisuals(musicButton, musicState);
    }

    public void Save()
    {
        Sijil.Save(this, sfxStateKey, sfxState);
        Sijil.Save(this, musicStateKey, musicState);
    }
}
