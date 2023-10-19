using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine.Networking;
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
    private string playerDataPath;
    private bool isLoadPlayerDone;
    private bool isLoadChapterDone;

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
            if (listFileDataNames[i].Contains(FileDataName.PLAYERDATA))
            {
                loadPlayerData(listFileDataNames[i]);
            }
            else if (listFileDataNames[i].Contains(FileDataName.LEVELDATA))
            {
                StartCoroutine(loadLevelData(listFileDataNames[i]));
            }
        }
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    public ChapterData GetChapterData()
    {
        return chapterData;
    }

    public void SetPlayerData(PlayerData _newPlayerData)
    {
        var jsonString = File.ReadAllText(playerDataPath);

        var data = JsonUtility.FromJson<PlayerData>(jsonString);

        data.CurrentLevel = _newPlayerData.CurrentLevel;
        data.CurrentGold = _newPlayerData.CurrentGold;
        data.HighScore = _newPlayerData.HighScore;
        data.IsSoundOn = _newPlayerData.IsSoundOn;

        jsonString = JsonUtility.ToJson(data);

        File.WriteAllText(playerDataPath, jsonString);
        playerData = data;
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

    private void loadPlayerData(string _nameFile)
    {
        var _filePath = Path.Combine(Application.persistentDataPath, $"{_nameFile}.json");
        playerDataPath = _filePath;
        string jsonData = File.Exists(playerDataPath) ? File.ReadAllText(playerDataPath) : "";
        if (string.IsNullOrEmpty(jsonData))
        {
            PlayerData defaultData = new PlayerData()
            {
                CurrentLevel = 1,
                CurrentGold = 0,
                HighScore = 0,
                IsSoundOn = true
            };

            jsonData = JsonUtility.ToJson(defaultData);
            File.WriteAllText(playerDataPath, jsonData);
        }
        playerData = JsonUtility.FromJson<PlayerData>(jsonData);
        isLoadPlayerDone = true;
    }

    private IEnumerator loadLevelData(string _nameFile)
    {
        chapterData = new ChapterData();
    
        var _filePath = Path.Combine(Application.streamingAssetsPath, $"{_nameFile}.json");

        UnityWebRequest www;

        if (Application.platform == RuntimePlatform.Android)
        {
            // Nếu đang chạy trên Android, sử dụng phương thức khác để đọc tệp từ StreamingAssets
            www = UnityWebRequest.Get("jar:file://" + _filePath);
        }
        else
        {
            // Đối với Editor và nền tảng khác, sử dụng đường dẫn thường
            www = UnityWebRequest.Get(_filePath);
        }

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("Error loading JSON: " + www.error);
        }
        else
        {
            var jsonData = www.downloadHandler.text;
            chapterData = JsonUtility.FromJson<ChapterData>(jsonData);
            isLoadChapterDone = true;
        }
    }

    public bool IsLoadDataDone()
    {
        return isLoadPlayerDone && isLoadChapterDone;
    }
}

public class FileDataName
{
    public const string PLAYERDATA = "playerdata";
    public const string LEVELDATA = "level";
}