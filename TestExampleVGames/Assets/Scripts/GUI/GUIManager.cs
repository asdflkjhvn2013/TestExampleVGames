using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GUIManager : MonoBehaviour, IGUIManager
{
    [SerializeField] private GameObject mainMenuGameObject;
    [SerializeField] private GameObject gamePlayGameObject;
    [SerializeField] private GameObject resultGameObject;
    [SerializeField] private GUIThankyou guiThanks;

    private IGuiItem mainMenu;
    private IGuiItem gamePlay;
    private IGuiItem guiResult;

    private Action onClickPlayGame;

    public void AssignEvent(Action _onPlayGame,PlayerData _playerData)
    {
        onClickPlayGame = _onPlayGame;

        mainMenu = mainMenuGameObject.GetComponent<IGuiItem>();
        gamePlay = gamePlayGameObject.GetComponent<IGuiItem>();
        guiResult = resultGameObject.GetComponent<IGuiItem>();

        mainMenu.Init(_playerData.CurrentLevel);
        gamePlay.Init(_playerData.CurrentLevel);
        guiResult.Init(_playerData.CurrentLevel);
        guiThanks.Init();

        EventHandle.OnPlayGame += onPlayGame;
        EventHandle.OnWinGame += onWinGame;
        EventHandle.OnMainMenu += onMainMenu;
    }

    private void onMainMenu()
    {
       mainMenuGameObject.SetActive(true);
       gamePlayGameObject.SetActive(false);
    }


    public void SetSelected(Vector3 _posSelection, int _idChess)
    {
        gamePlay.SetSelected(_posSelection, _idChess);
    }

    public void OpenUIThanks()
    {
        mainMenu.OnWinGame(1);
        guiThanks.gameObject.SetActive(true);
    }

    private void onPlayGame()
    {
        mainMenuGameObject.SetActive(false);
        gamePlayGameObject.SetActive(true);
        onClickPlayGame?.Invoke();
    }

    private void onWinGame(int _level)
    {
        mainMenu.OnWinGame(_level);
        gamePlay.OnWinGame(_level);
        guiResult.OnWinGame(_level);
    }


    private void OnDestroy()
    {
        EventHandle.OnPlayGame -= onPlayGame;
        EventHandle.OnWinGame -= onWinGame;
        EventHandle.OnMainMenu -= onMainMenu;
    }
}