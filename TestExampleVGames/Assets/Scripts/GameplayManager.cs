using System;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private IDataSystem dataSystemSystem;
    private ISpawner spawnerManager;
    private IGUIManager guiManager;

    private PlayerData playerData;
    
    private string filePath = Application.dataPath + "/Data/data.json";

    private void Awake()
    {
        dataSystemSystem = GetComponentInChildren<IDataSystem>();
        spawnerManager = GetComponentInChildren<ISpawner>();
        guiManager = GetComponentInChildren<IGUIManager>();
        
        guiManager.AssignEvent(onClickPlayGame);
    }

    private void Start()
    {
        if (!dataSystemSystem.CheckingData(filePath))
        {
         dataSystemSystem.InitialData(playerData);
        }

        playerData = dataSystemSystem.FetchData();
        
    }
    
    private void onClickPlayGame()
    {
       spawnerManager.SpawnerAt(playerData.CurrentLevel);
       Debug.Log("Spawned");
    }

}
