using UnityEngine;

public interface IData
{
    bool CheckingData();
    void InitialData();
    PlayerData FetchData();
    void OverrideData(PlayerData _playerData);
}
public interface ISpawner
{
    void SpawnerAt(int _level);
    void ReSpawn();
}