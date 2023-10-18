using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GUIMainMenu : MonoBehaviour,IGuiItem
{
    [SerializeField] private Button btnPlayGame;
    [SerializeField] private TextMeshProUGUI txtLevel;

    private void OnDestroy()
    {
        btnPlayGame.onClick.RemoveListener(onClickPlayGame);
    }

    private void onClickPlayGame()
    {
        EventHandle.OnPlayGame.Invoke();
    }

    public void Init(int _level)
    {
        btnPlayGame.onClick.AddListener(onClickPlayGame);
        txtLevel.SetText(_level.ToString());
    }
    
    public void SetSelected(Vector3 _posSelection, int _idChess)
    {
        
    }

    public void OnWinGame(int _levelNext)
    {
        txtLevel.SetText(_levelNext.ToString());
    }
}
