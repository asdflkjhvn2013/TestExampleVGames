using System;
using System.Collections.Generic;
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
    void AssignData(ChapterData _chapterData);
    void SpawnerAt(int _level);
    List<GameObject> GetListChess();
    void ReSpawn();
}

public interface IGUIManager
{
    void AssignEvent(Action _onPlayGame,PlayerData _playerData);
    void SetSelected(Vector3 _posSelection, int _idChess);
    void OpenUIThanks();
}

public interface IGuiItem
{
    void Init(int _level);
    void SetSelected(Vector3 _posSelection, int _idChess);
    void OnWinGame(int _levelNext);
}

public interface IActionHandle
{
    void Initialize();
    void AssignEvent(Action<Vector2> _onMouseDown, Action<Vector2> _onMouseUp);
}

public interface IOutlineHandle
{
    void SetOutline();
    void UnOutline();
}

public interface ISelectionHandle
{
    void SetSelected(Action<Vector3, int> _callback);
}

public interface IChessItem
{
    void AssignData(int _id);
}