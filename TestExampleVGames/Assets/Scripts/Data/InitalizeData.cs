using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitalizeData : MonoBehaviour,IData
{
    private string filePath = Application.dataPath + "/Data/data.json";
    
    public bool CheckingData()
    {
        if (!System.IO.File.Exists(filePath))
        {
            return false;
        }

        return true;
    }

    public void InitialData()
    {
        PlayerData defaultData = new PlayerData()
        {
            CurrentLevel = 1,
            CurrentGold = 0,
            HighScore = 0,
            IsSoundOn = true
        };

        string jsonData = JsonUtility.ToJson(defaultData);
        System.IO.File.WriteAllText(filePath,jsonData);
    }

    public PlayerData FetchData()
    {
        string jsonData = System.IO.File.ReadAllText(filePath);

        PlayerData playerData = JsonUtility.FromJson<PlayerData>(jsonData);
        
        return playerData;
    }

    public void OverrideData(PlayerData _playerData)
    {
        throw new System.NotImplementedException();
    }
}
