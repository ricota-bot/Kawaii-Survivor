using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Elements")]
    public static GameManager instance;

    [Header("Actions")]
    public static Action onGamePaused;
    public static Action onGameResumed;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        SetGameState(GameState.MENU);
    }

    public void StartMenu() => SetGameState(GameState.MENU);
    public void StartGame() => SetGameState(GameState.GAME);
    public void StartShop() => SetGameState(GameState.SHOP);
    public void StartWeaponSelection() => SetGameState(GameState.WEAPONSELECTION);
    public void ManageGameover() => SceneManager.LoadScene(0);


    public void PauseButtonCallBack()
    {
        Time.timeScale = 0;
        onGamePaused?.Invoke();
    }

    public void ResumeButtonCallBack()
    {
        Time.timeScale = 1;
        onGameResumed?.Invoke();
    }

    public void RestartFromPause()
    {
        Time.timeScale = 1;
        ManageGameover();
    }

    public void WaveCompleteCallBack()
    {
        if (Player.instance.HasLeveledUp() || WaveTransitionManager.Instance.HasCollectedChest())
        {
            SetGameState(GameState.WAVETRANSITION);
        }
        else
            SetGameState(GameState.SHOP);
    }

    // SET GAMES STATES
    public void SetGameState(GameState state)
    {
        IEnumerable<IGameStateListener> gameStateListener = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IGameStateListener>();

        foreach (IGameStateListener listener in gameStateListener)
            listener.OnGameStateChangedCallBack(state);
    }
}

public interface IGameStateListener
{
    void OnGameStateChangedCallBack(GameState state);
}
