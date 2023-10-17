using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MEC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIGamePlay : MonoBehaviour, IGuiItem
{
    [SerializeField] private TextMeshProUGUI txtTimer;
    [SerializeField] private TextMeshProUGUI txtLevel;
    [SerializeField] private GUIChessSelectedItem chessSelectedPrefab;
    [SerializeField] private Transform tfSlot;
    [SerializeField] private List<GameObject> listSlot;

    private Sprite[] sprites;
    [SerializeField] private List<GUIChessSelectedItem> listChessSelected;

    private int countselected = 0;

    #region IGuiItem

    public void Init()
    {
        listChessSelected = new List<GUIChessSelectedItem>();

        EventHandle.OnPlayGame += onPlayGame;
        EventHandle.OnCheckMatchStart += onWinMatch;

        loadSpriteChess();
    }

    public void SetSelected(Vector3 _posSelection, int _idChess)
    {
        var chessSelected = createUIChessSelected(_posSelection, _idChess);
        var rectSlot = listSlot[countselected].GetComponent<RectTransform>();
        chessSelected.MoveToSlot(rectSlot.anchoredPosition, () => { countselected++; });
    }

    #endregion

    #region Function

    private void loadSpriteChess()
    {
        sprites = Resources.LoadAll<Sprite>("MahjongTile");
    }

    private GUIChessSelectedItem createUIChessSelected(Vector3 _posSelection, int _idChess)
    {
        var posCanvas = Camera.main.WorldToScreenPoint(_posSelection);
        var chessSelected = Instantiate(chessSelectedPrefab, tfSlot);

        chessSelected.Init(posCanvas, sprites[_idChess], countselected, _idChess);

        listChessSelected.Add(chessSelected);

        return chessSelected;
    }

    #endregion

    #region EventHadle

    private void onPlayGame()
    {
    }

    private void onWinMatch(int _idMatch)
    {
        Timing.RunCoroutine(coroutineWinMatch(_idMatch));
    }

    private IEnumerator<float> coroutineWinMatch(int _idMatch)
    {
        yield return Timing.WaitForSeconds(0.5f);
        
        for (var i = 0; i < listChessSelected.Count; i++)
        {
            var chessSelected = listChessSelected[i];
            if (chessSelected.GetIdChess() == _idMatch)
            {
                chessSelected.ScaleWinMatch();
            }
        }

        yield return Timing.WaitForSeconds(0.5f);

        for (var i = listChessSelected.Count - 1; i >= 0; i--)
        {
            var chessSelected = listChessSelected[i];

            if (chessSelected.GetIdChess() == _idMatch)
            {
                listChessSelected.RemoveAt(i);
                Destroy(chessSelected.gameObject);
            }
        }

        countselected = 0;
        EventHandle.OnCheckMatchDone.Invoke();
    }

    #endregion

    private void OnDestroy()
    {
        EventHandle.OnPlayGame -= onPlayGame;
    }
}