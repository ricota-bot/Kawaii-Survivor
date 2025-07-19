using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStatsManager))]
public class PlayerObjects : MonoBehaviour
{
    [field: SerializeField] public List<ObjectDataSO> ObjectsData { get; private set; }
    private PlayerStatsManager _playerStatsManager;


    private void Awake()
    {
        _playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    private void Start()
    {
        foreach (var objData in ObjectsData)
            _playerStatsManager.AddObjectData(objData.BaseStats);
    }


    public void AddObjects(ObjectDataSO objectDataToTake)
    {
        ObjectsData.Add(objectDataToTake);
        _playerStatsManager.AddObjectData(objectDataToTake.BaseStats);

    }
}
