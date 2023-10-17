using System;
using System.Collections.Generic;
using System.Linq;
using MEC;
using UnityEditor;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayerMask;

    private ISpawner spawnerManager;
    private IGUIManager guiManager;
    private IActionHandle inputManager;
    private OutlineManager outlineManager;

    private RaycastHit hit;
   [SerializeField] private List<int> chessHasSelected;
    private bool isHasMatch;
    private int idMatch;

    private void Start()
    {
        spawnerManager = GetComponentInChildren<ISpawner>();
        guiManager = GetComponentInChildren<IGUIManager>();
        inputManager = GetComponentInChildren<IActionHandle>();
        outlineManager = GetComponentInChildren<OutlineManager>();

        DataManager.INTANCE.LoadData();
        inputManager.Initialize();
        chessHasSelected = new List<int>();
        EventHandle.OnCheckMatchDone += onCheckMatchDone;

        spawnerManager.AssignData(DataManager.INTANCE.GetChapterData());
        guiManager.AssignEvent(onClickPlayGame);
        inputManager.AssignEvent(onMouseDown, onMouseUp);
        Timing.RunCoroutine(updateGameplay());
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
        spawnerManager.SpawnerAt(DataManager.INTANCE.GetPlayerData().CurrentLevel);
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
                    idMatch = group.Key;
                    EventHandle.OnCheckMatchStart.Invoke(group.Key);
                }
            }
        }
    }

    private void OnDestroy()
    {
        EventHandle.OnCheckMatchDone -= onCheckMatchDone;
    }
}