using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GUIManager : MonoBehaviour, IGUIManager
{
    [SerializeField] private GameObject mainMenuGameObject;
    [SerializeField] private GameObject gamePlayGameObject;

    private IGuiItem mainMenu;
    private IGuiItem gamePlay;

    private Action onClickPlayGame;
    
    public void AssignEvent(Action _onPlayGame)
    {
        onClickPlayGame = _onPlayGame;
        
        mainMenu = mainMenuGameObject.GetComponent<IGuiItem>();
        gamePlay = gamePlayGameObject.GetComponent<IGuiItem>();
        
        mainMenu.Init();
        gamePlay.Init();
        
        EventHandle.OnPlayGame += onPlayGame;
    }

    public void SetSelected(Vector3 _posSelection, int _idChess)
    {
        gamePlay.SetSelected(_posSelection, _idChess);
    }

    private void onPlayGame()
    {
        mainMenuGameObject.SetActive(false);
        gamePlayGameObject.SetActive(true);
        onClickPlayGame?.Invoke();
    }

    private void OnDestroy()
    {
        EventHandle.OnPlayGame -= onPlayGame;
    }
}