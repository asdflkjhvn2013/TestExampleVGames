using System;
using UnityEngine;

public interface IDataSystem
{
    bool CheckingData(string _filePath);
    void InitialData(PlayerData _playerData);
    PlayerData FetchData();
    void OverrideData(PlayerData _playerData);
}
public interface ISpawner
{
    void SpawnerAt(int _level);
    void ReSpawn();
}

public interface IGUIManager
{
    void AssignEvent(Action _onPlayGame);
}