using UnityEngine;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private PlayerWeapons _playerWeapons;
    [SerializeField] private WeaponContainer _weaponContainerPrefab;
    [SerializeField] private Transform _containerParent;


    [Header("DATA")]
    [SerializeField] private WeaponDataSO[] starterWeapons;
    private WeaponDataSO _selectedWeapon;
    private int _initialWeaponLevel;

    public void OnGameStateChangedCallBack(GameState state)
    {
        switch (state)
        {
            case GameState.GAME:
                if (_selectedWeapon == null)
                    return;

                _playerWeapons.AddWeapons(_selectedWeapon, _initialWeaponLevel);

                _selectedWeapon = null;
                _initialWeaponLevel = 0;
                break;

            case GameState.WEAPONSELECTION:
                Configure();

                break;
        }
    }

    [NaughtyAttributes.Button]
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

        WeaponDataSO weaponData = starterWeapons[Random.Range(0, starterWeapons.Length)];

        int level = Random.Range(0, 4);
        weaponContainerInstance.Configure(weaponData.Sprite, weaponData.WeaponName, level, weaponData);

        weaponContainerInstance.Button.onClick.RemoveAllListeners();
        weaponContainerInstance.Button.onClick.AddListener(() => WeaponSelectedCallBack(weaponContainerInstance, weaponData, level));
    }


    private void WeaponSelectedCallBack(WeaponContainer weaponContainer, WeaponDataSO weaponData, int level)
    {
        _selectedWeapon = weaponData;
        _initialWeaponLevel = level;

        foreach (WeaponContainer container in _containerParent.GetComponentsInChildren<WeaponContainer>())
        {
            if (container == weaponContainer) // O que estamos comparando é "weaponContainer => Parametro" comparando com container, todos os filhos de containerParent
                container.Select(); // Caso sejam iguais, para chamar o metodo de Selecionar
            else
                container.Deselect();
        }
    }
}
