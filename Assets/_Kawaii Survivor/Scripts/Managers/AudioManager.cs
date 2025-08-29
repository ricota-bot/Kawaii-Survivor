using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public bool IsSFXOn { get; private set; }
    public bool IsMusicOn { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        SettingsManager.OnSFXStateChanged += OnSFXStateChangedCallBack;
        SettingsManager.OnMusicStateChanged += OnMusicStateChangedCallBack;
    }

    private void OnDestroy()
    {
        SettingsManager.OnSFXStateChanged -= OnSFXStateChangedCallBack;
        SettingsManager.OnMusicStateChanged -= OnMusicStateChangedCallBack;
    }
    private void OnSFXStateChangedCallBack(bool sfxState)
    {
        IsSFXOn = sfxState;
    }

    private void OnMusicStateChangedCallBack(bool musicState)
    {
        IsMusicOn = musicState;
    }



}
