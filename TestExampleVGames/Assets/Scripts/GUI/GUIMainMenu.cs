using System;
using UnityEngine;
using UnityEngine.UI;


public class GUIMainMenu : MonoBehaviour,IGuiItem
{
    [SerializeField] private Button btnPlayGame;

    private void OnDestroy()
    {
        btnPlayGame.onClick.RemoveListener(onClickPlayGame);
    }

    private void onClickPlayGame()
    {
        EventHandle.OnPlayGame.Invoke();
    }

    public void Init()
    {
        btnPlayGame.onClick.AddListener(onClickPlayGame);
    }
    
    public void SetSelected(Vector3 _posSelection, int _idChess)
    {
        
    }
}
