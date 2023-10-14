using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GUIManager : MonoBehaviour, IGUIManager
{
    [SerializeField] private GameObject mainMenuGameObject;
    [SerializeField] private GameObject gamePlayGameObject;

    private IGUIManager mainMenu;
    private IGUIManager gamePlay;

    private Action onClickPlayGame;

    private void Awake()
    {
        mainMenu = mainMenuGameObject.GetComponent<IGUIManager>();
        gamePlay = gamePlayGameObject.GetComponent<IGUIManager>();
    }

    public void AssignEvent(Action _onPlayGame)
    {
        onClickPlayGame = _onPlayGame;
        mainMenu.AssignEvent(onPlayGame);
    }

    private void onPlayGame()
    {
        mainMenuGameObject.SetActive(false);
        gamePlayGameObject.SetActive(true);
        onClickPlayGame?.Invoke();
    }
}