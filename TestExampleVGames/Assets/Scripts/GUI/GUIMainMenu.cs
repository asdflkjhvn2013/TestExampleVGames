using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GUIMainMenu : MonoBehaviour,IGUIManager
{
    [SerializeField] private Button btnPlayGame;
    private Action onPlayGame;

    private void Start()
    {
        btnPlayGame.onClick.AddListener(onClickPlayGame);
    }

    private void OnDestroy()
    {
        btnPlayGame.onClick.RemoveListener(onClickPlayGame);
    }

    private void onClickPlayGame()
    {
        onPlayGame?.Invoke();
    }

    public void AssignEvent(Action _onPlayGame)
    {
        onPlayGame = _onPlayGame;
    }
    
}
