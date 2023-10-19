using System;
using System.Collections.Generic;
using System.Linq;
using MEC;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayerMask;

    private ISpawner spawnerManager;
    private IGUIManager guiManager;
    private IActionHandle inputManager;
    private OutlineManager outlineManager;

    private RaycastHit hit;
    private List<int> chessHasSelected;
    private bool isHasMatch;
    private int idMatch;
    private int score;
    private int numOfSlot = 8;
    private CoroutineHandle checkLoadDataDone;
    
    private void Awake()
    {
        chessHasSelected = new List<int>();

        spawnerManager = GetComponentInChildren<ISpawner>();
        guiManager = GetComponentInChildren<IGUIManager>();
        inputManager = GetComponentInChildren<IActionHandle>();
        outlineManager = GetComponentInChildren<OutlineManager>();
        
        DataManager.INTANCE.LoadData();
        inputManager.Initialize();

        EventHandle.OnCheckMatchDone += onCheckMatchDone;
        EventHandle.OnNextLevel += onClickPlayGame;
        EventHandle.OnMainMenu += onMainMenu;

       checkLoadDataDone = Timing.RunCoroutine(checkDataLoadDone());
        
        inputManager.AssignEvent(onMouseDown, onMouseUp);
        
        Timing.RunCoroutine(updateGameplay());
    }

    private IEnumerator<float> checkDataLoadDone()
    {
        while (true)
        {
            if (DataManager.INTANCE.IsLoadDataDone())
            {
                spawnerManager.AssignData(DataManager.INTANCE.GetChapterData());
                guiManager.AssignEvent(onClickPlayGame, DataManager.INTANCE.GetPlayerData());
                Timing.KillCoroutines(checkLoadDataDone);
            }
            yield return Timing.WaitForOneFrame;
        }
    }

    private void onMainMenu()
    {
        if (chessHasSelected.Count > 0)
        {
            chessHasSelected.Clear();
        }

        score = 0;
    }

    private void onCheckMatchDone()
    {
        for (var i = chessHasSelected.Count - 1; i >= 0; i--)
        {
            if (chessHasSelected[i] == idMatch)
            {
                chessHasSelected.RemoveAt(i);
            }
        }

        isHasMatch = false;
    }

    private IEnumerator<float> updateGameplay()
    {
        while (true)
        {
            checkMathChess();
            yield return Timing.WaitForOneFrame;
        }
    }

    private void onMouseDown(Vector2 _pos)
    {
        if (CheckOutlineAvailable(_pos))
        {
            outlineManager.SetOutline(hit.transform.gameObject);
        }
        else
        {
            outlineManager.UnOutline();
        }
    }

    private void onMouseUp(Vector2 _pos)
    {
        if (CheckOutlineAvailable(_pos))
        {
            var _selection = hit.transform.gameObject.GetComponent<ISelectionHandle>();
            _selection.SetSelected((_posSelection, _idChess) =>
            {
                guiManager.SetSelected(_posSelection, _idChess);
                chessHasSelected.Add(_idChess);
            });
            outlineManager.UnOutline();
        }
    }

    private void onClickPlayGame()
    {
        PlayerData playerData = DataManager.INTANCE.GetPlayerData();
        if (playerData.CurrentLevel <= 3)
        {
            spawnerManager.SpawnerAt(playerData.CurrentLevel);
            var leveldata = DataManager.INTANCE.GetLevelData(playerData.CurrentLevel);
            EventHandle.OnStarCountTimer.Invoke(leveldata.timePlayLevel);
        }
        else
        {
            guiManager.OpenUIThanks();
        }
    }

    private bool CheckOutlineAvailable(Vector2 _pos)
    {
        Ray _ray = Camera.main.ScreenPointToRay(_pos);
        if (Physics.Raycast(_ray, out hit, float.MaxValue, targetLayerMask))
        {
            return true;
        }

        return false;
    }

    private void checkMathChess()
    {
        if (chessHasSelected != null && chessHasSelected.Count >= 3)
        {
            if (isHasMatch)
            {
                return;
            }

            isHasMatch = true;
            var groupIds = chessHasSelected.GroupBy(i => i);

            foreach (var group in groupIds)
            {
                if (group.Count() >= 3)
                {
                    var levelData = DataManager.INTANCE.GetLevelData(DataManager.INTANCE.GetPlayerData().CurrentLevel);
                    score += levelData.scorePerMatch;

                    idMatch = group.Key;

                    EventHandle.OnCheckMatchStart.Invoke(group.Key, score);
                }
                else
                {
                    isHasMatch = false;
                }
            }

            checkWinLevel();
        }
    }

    private void checkWinLevel()
    {
        if (chessHasSelected.Count < numOfSlot)
        {
            var listChessSpawn = spawnerManager.GetListChess();
            if (listChessSpawn.All(chess => !chess.activeSelf))
            {
                var playerData = DataManager.INTANCE.GetPlayerData();
                PlayerData _newPlayerData = new PlayerData()
                {
                    CurrentLevel = playerData.CurrentLevel + 1,
                    CurrentGold = 0,
                    HighScore = score > playerData.HighScore ? score : playerData.HighScore,
                    IsSoundOn = playerData.IsSoundOn
                };

                DataManager.INTANCE.SetPlayerData(_newPlayerData);

                EventHandle.OnWinGame.Invoke(_newPlayerData.CurrentLevel);
                score = 0;
            }
        }
        else
        {
            EventHandle.OnTimeOut.Invoke();
        }
    }

    private void OnDestroy()
    {
        EventHandle.OnCheckMatchDone -= onCheckMatchDone;
        EventHandle.OnNextLevel -= onClickPlayGame;
        EventHandle.OnMainMenu -= onMainMenu;
    }
}