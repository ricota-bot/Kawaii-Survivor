using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _weaponSelectionPanel;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _stageCompletePanel;
    [SerializeField] private GameObject _waveTransitionPanel;
    [SerializeField] private GameObject _shopPanel;

    [SerializeField] private GameObject _restartConfirmationPanel;
    [SerializeField] private GameObject _characterSelectionPanel;
    [SerializeField] private GameObject _settingsPanel;

    private List<GameObject> _panels = new List<GameObject>();


    private void Awake()
    {
        _panels.AddRange(new GameObject[]
        {
            _menuPanel,
            _weaponSelectionPanel,
            _gamePanel,
            _gameOverPanel,
            _stageCompletePanel,
            _waveTransitionPanel,
            _shopPanel
        });

        GameManager.onGamePaused += onGamePausedCallBack;
        GameManager.onGameResumed += onGameResumedCallBack;

        _pausePanel.SetActive(false);
        _restartConfirmationPanel.SetActive(false);


        HideCharacterSelectionPanel();
        HideSettingsPanel();
    }

    private void OnDestroy()
    {
        GameManager.onGamePaused -= onGamePausedCallBack;
        GameManager.onGameResumed -= onGameResumedCallBack;
    }

    public void OnGameStateChangedCallBack(GameState state)
    {
        switch (state)
        {
            case GameState.MENU:
                ShowPanel(_menuPanel);
                break;

            case GameState.WEAPONSELECTION:
                ShowPanel(_weaponSelectionPanel);
                break;

            case GameState.GAME:
                ShowPanel(_gamePanel);
                break;

            case GameState.GAMEOVER:
                ShowPanel(_gameOverPanel);
                break;
            case GameState.STAGECOMPLETE:
                ShowPanel(_stageCompletePanel);
                break;

            case GameState.WAVETRANSITION:
                ShowPanel(_waveTransitionPanel);
                break;

            case GameState.SHOP:
                ShowPanel(_shopPanel);
                break;
        }
    }

    private void ShowPanel(GameObject panel)
    {
        foreach (GameObject item in _panels)
        {
            item.SetActive(item == panel);
        }

    }

    public void ShowRestartConfirmationPanel()
    {
        _restartConfirmationPanel.SetActive(true);

        RectTransform panel = _restartConfirmationPanel.transform
                              .Find("Container")
                              .GetComponent<RectTransform>();

        panel.localScale = Vector3.zero;

        LeanTween.cancel(panel);
        LeanTween.scale(panel, Vector3.one, 0.25f)
                 .setEaseOutBack()
                 .setIgnoreTimeScale(true); // roda mesmo em pause
    }
    public void HideRestartConfirmationPanel()
    {
        RectTransform panel = _restartConfirmationPanel.transform
                              .Find("Container")
                              .GetComponent<RectTransform>();

        LeanTween.cancel(panel);
        LeanTween.scale(panel, Vector3.zero, 0.25f)
                 .setEaseInBack()
                 .setIgnoreTimeScale(true)
                 .setOnComplete(() => _restartConfirmationPanel.SetActive(false));

    }
    public void ShowCharacterSelectionPanel() => _characterSelectionPanel.SetActive(true);
    public void HideCharacterSelectionPanel() => _characterSelectionPanel.SetActive(false);

    public void ShowSettingsPanel() => _settingsPanel.SetActive(true);
    public void HideSettingsPanel() => _settingsPanel.SetActive(false);


    // MY ACTIONS
    private void onGamePausedCallBack() => _pausePanel.SetActive(true);
    private void onGameResumedCallBack() => _pausePanel.SetActive(false);


}
