using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerLevel : MonoBehaviour
{
    [Header("Settings")]
    private int requireXP;
    private int currentXP;
    private int level;
    private int levelEarnedInThisWave;

    [Header("Visuals")]
    [SerializeField] private Slider _xpBar;
    [SerializeField] private TextMeshProUGUI _levelText;

    [Header("DEBUG")]
    [SerializeField] private bool DEBUG;

    private void Awake()
    {
        Candy.OnDropCollected += OnDropCollectedCallBack;
    }

    private void OnDestroy()
    {
        Candy.OnDropCollected -= OnDropCollectedCallBack;

    }

    private void Start()
    {
        UpdateRequireXP();
        UpdateVisuals();
    }

    private void UpdateRequireXP() =>
        requireXP = (level + 1) * 5;


    private void UpdateVisuals()
    {
        _xpBar.value = (float)currentXP / requireXP;
        _levelText.text = $"Level {level + 1}";
    }

    private void OnDropCollectedCallBack(Candy candy)
    {
        currentXP++;

        if (currentXP >= requireXP)
            LevelUp();

        UpdateVisuals();
    }

    private void LevelUp()
    {
        level++;
        currentXP = 0;
        levelEarnedInThisWave++;
        UpdateRequireXP();
    }

    public bool HasLeveledUp()
    {
        if (DEBUG)
            return true;

        if (levelEarnedInThisWave > 0)
        {
            levelEarnedInThisWave--;
            return true;
        }

        return false;
    }
}
