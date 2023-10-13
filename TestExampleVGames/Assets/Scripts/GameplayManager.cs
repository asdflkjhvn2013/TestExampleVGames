using System;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private IData dataSystem;
    private ISpawner spawnerManager;

    private PlayerData playerData;

    private void Awake()
    {
        dataSystem = GetComponentInChildren<IData>();
        spawnerManager = GetComponentInChildren<ISpawner>();
    }

    private void Start()
    {
        if (!dataSystem.CheckingData())
        {
          dataSystem.InitialData();
        }

        playerData = dataSystem.FetchData();
        
        Debug.Log($"level: {playerData.CurrentLevel} - Gold: {playerData.CurrentGold}");
    }
}
