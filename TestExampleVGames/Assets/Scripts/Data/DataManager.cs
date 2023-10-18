using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.TextCore;

public class DataManager : MonoBehaviour
{
    private static DataManager intance;

    public static DataManager INTANCE
    {
        get => intance;
    }

    [SerializeField] private List<string> listFileDataNames;

    [SerializeField] private PlayerData playerData;
    [SerializeField] private ChapterData chapterData;

    private void Awake()
    {
        if (intance == null)
        {
            intance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadData()
    {
        for (int i = 0; i < listFileDataNames.Count; i++)
        {
            var filePath = Application.dataPath + "/Data/" + listFileDataNames[i] + ".json";
            if (filePath.Contains(FileDataName.PLAYERDATA))
            {
                loadPlayerData(filePath);
            }
            else if (filePath.Contains(FileDataName.LEVELDATA))
            {
                loadLevelData(filePath);
            }
        }
    }

    public PlayerData GetPlayerData()
    {
        for (int i = 0; i < listFileDataNames.Count; i++)
        {
            var filePath = Application.dataPath + "/Data/" + listFileDataNames[i] + ".json";
            if (filePath.Contains(FileDataName.PLAYERDATA))
            {
                loadPlayerData(filePath);
            }
        }

        return playerData;
    }

    public ChapterData GetChapterData()
    {
        return chapterData;
    }

    public void SetPlayerData(PlayerData _newPlayerData)
    {
        for (int i = 0; i < listFileDataNames.Count; i++)
        {
            var filePath = Application.dataPath + "/Data/" + listFileDataNames[i] + ".json";
            if (filePath.Contains(FileDataName.PLAYERDATA))
            {
                var jsonString = File.ReadAllText(filePath);

                var data = JsonUtility.FromJson<PlayerData>(jsonString);

                data.CurrentLevel = _newPlayerData.CurrentLevel;
                data.CurrentGold = _newPlayerData.CurrentGold;
                data.HighScore = _newPlayerData.HighScore;
                data.IsSoundOn = _newPlayerData.IsSoundOn;

                jsonString = JsonUtility.ToJson(data);

                File.WriteAllText(filePath, jsonString);
            }
        }
    }

    public LevelData GetLevelData(int _level)
    {
        var levelData = chapterData.chapter.FirstOrDefault(i => i.level == _level);

        if (_level != null)
        {
            return levelData;
        }

        return null;
    }

    private void loadPlayerData(string _filePath)
    {
        string jsonData = "";
        if (!File.Exists(_filePath))
        {
            PlayerData defaultData = new PlayerData()
            {
                CurrentLevel = 1,
                CurrentGold = 0,
                HighScore = 0,
                IsSoundOn = true
            };

            jsonData = JsonUtility.ToJson(defaultData);
            File.WriteAllText(_filePath, jsonData);
        }

        jsonData = File.ReadAllText(_filePath);
        playerData = JsonUtility.FromJson<PlayerData>(jsonData);
    }

    private void loadLevelData(string _filePath)
    {
        chapterData = new ChapterData();

        var jsonData = File.ReadAllText(_filePath);

        chapterData = JsonUtility.FromJson<ChapterData>(jsonData);
    }
}

public class FileDataName
{
    public const string PLAYERDATA = "playerdata";
    public const string LEVELDATA = "level";
}