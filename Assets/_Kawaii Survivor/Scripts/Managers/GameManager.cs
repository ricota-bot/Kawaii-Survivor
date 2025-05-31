using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Elements")]
    public static GameManager instance;
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

    public void StartGame() => SetGameState(GameState.GAME);
    public void StartWeaponSelection() => SetGameState(GameState.WEAPONSELECTION);
    public void StartShop() => SetGameState(GameState.SHOP);
    public void ManageGameover() => SceneManager.LoadScene(0);


    public void SetGameState(GameState state)
    {
        IEnumerable<IGameStateListener> gameStateListener = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IGameStateListener>();

        foreach (IGameStateListener listener in gameStateListener)
            listener.OnGameStateChangedCallBack(state);
    }

    public void WaveCompleteCallBack()
    {
        if (Player.instance.HasLeveledUp())
        {
            SetGameState(GameState.WAVETRANSITION);
        }
        else
            SetGameState(GameState.SHOP);
    }
}

public interface IGameStateListener
{
    void OnGameStateChangedCallBack(GameState state);
}
