using UnityEngine;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private WeaponContainer _weaponContainerPrefab;
    [SerializeField] private Transform _containerParent;

    public void OnGameStateChangedCallBack(GameState state)
    {
        switch (state)
        {
            case GameState.WEAPONSELECTION:
                Configure();

                break;
        }
    }


    private void Configure()
    {
        // Clean our Parent, no children
        _containerParent.Clear();

        for (int i = 0; i < 3; i++)
            GenerateWeaponsContainers();

    }

    private void GenerateWeaponsContainers()
    {
        WeaponContainer weaponContainerInstance = Instantiate(_weaponContainerPrefab, _containerParent);
    }
}
