using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Button[] upgradeContainers;


    public void OnGameStateChangedCallBack(GameState state)
    {
        switch (state)
        {
            case GameState.WAVETRANSITION:
                ConfigureUpgradeContainers();
                break;
        }
    }

    private void ConfigureUpgradeContainers()
    {
        for (int i = 0; i < upgradeContainers.Length; i++)
        {
            upgradeContainers[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Upgrade {i}";
        }

    }

}
