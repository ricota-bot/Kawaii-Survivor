using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _weaponSelectionPanel;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _stageCompletePanel;
    [SerializeField] private GameObject _waveTransitionPanel;
    [SerializeField] private GameObject _shopPanel;

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
}
