using System.Collections.Generic;
using UnityEngine;

public class StatContainerManager : MonoBehaviour
{
    public static StatContainerManager Instance;

    [Header("Elements")]
    [SerializeField] private StatContainer _statContainerPrefab;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    private void GenerateContainers(Dictionary<Stat, float> statDictionary, Transform parent)
    {
        List<StatContainer> statContainerList = new List<StatContainer>();

        foreach (KeyValuePair<Stat, float> kvp in statDictionary)
        {
            StatContainer statContainerInstance = Instantiate(_statContainerPrefab, parent);
            statContainerList.Add(statContainerInstance);

            Sprite icon = ResourcesManager.GetStatIcon(kvp.Key);
            string statName = Enums.FormatStatName(kvp.Key);
            float statValue = kvp.Value;

            statContainerInstance.Configure(icon, statName, statValue);
        }

        LeanTween.delayedCall(Time.deltaTime * 2, () => ResizeTexts(statContainerList)); // Wait "two" seconds befora call the method
    }

    private void ResizeTexts(List<StatContainer> statContainerList)
    {
        float minFontSize = 5000;

        for (int i = 0; i < statContainerList.Count; i++)
        {
            StatContainer statContainer = statContainerList[i];
            float fontSize = statContainer.GetFontSize();

            if (fontSize < minFontSize)
                minFontSize = fontSize;
        }
        // At this point, we have the min font size setup

        // Set This font size for all of the StatNamesText or StatValueText or the same :)
        for (int i = 0; i < statContainerList.Count; i++)
            statContainerList[i].SetFontSize(minFontSize);

    }


    public static void GenerateStatContainers(Dictionary<Stat, float> statDictionary, Transform parent)
    {
        Instance.GenerateContainers(statDictionary, parent);
    }


}
