using UnityEngine;

public static class ResourcesManager
{
    const string statIconDataPath = "Data/Stat Icons";

    private static StatIcon[] statIcons;
    public static Sprite GetStatIcon(Stat stat)
    {
        if (statIcons == null)
        {

            StatIconDataSO data = Resources.Load<StatIconDataSO>(statIconDataPath);
            statIcons = data.StatIcons;
        }

        foreach (StatIcon icon in statIcons)
        {
            if (stat == icon.stat)
                return icon.icon;
        }

        Debug.LogError("No icon Found for Stat: " + stat);
        return null;
    }

}
