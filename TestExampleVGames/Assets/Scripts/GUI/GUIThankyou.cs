using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIThankyou : MonoBehaviour
{
    [SerializeField] private Button btnPlayAgain;

    public void Init()
    {
        btnPlayAgain.onClick.AddListener(onClickPlayAgain);
    }

    private void onClickPlayAgain()
    {
        PlayerData playerData = new PlayerData()
        {
            CurrentLevel = 1,
            CurrentGold = 0,
            HighScore = 0,
            IsSoundOn = true
        };
        
        DataManager.INTANCE.SetPlayerData(playerData);
        
        EventHandle.OnMainMenu.Invoke();
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        btnPlayAgain.onClick.RemoveListener(onClickPlayAgain);
    }
}
