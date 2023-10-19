using System.Collections.Generic;
using DG.Tweening;
using MEC;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GUIGamePlay : MonoBehaviour, IGuiItem
{
    [SerializeField] private Button btnMainMenu;
    [SerializeField] private TextMeshProUGUI txtTimer;
    [SerializeField] private TextMeshProUGUI txtScore;
    [SerializeField] private GUIChessSelectedItem chessSelectedPrefab;
    [SerializeField] private Transform tfSlot;
    [SerializeField] private List<GameObject> listSlot;

    private Sprite[] sprites;
    private List<GUIChessSelectedItem> listChessSelected;
    private int countselected = 0;
    private int score;
    private CoroutineHandle timerHandle;

    #region IGuiItem

    public void Init(int _level)
    {
        listChessSelected = new List<GUIChessSelectedItem>();

        btnMainMenu.onClick.AddListener(onClickMainMenu);
        txtScore.SetText("0");

        EventHandle.OnStarCountTimer += onStartCountTimer;
        EventHandle.OnCheckMatchStart += onWinMatch;
        EventHandle.OnMainMenu += onMainMenu;

        loadSpriteChess();
    }

    private void onClickMainMenu()
    {
        EventHandle.OnMainMenu.Invoke();
    }

    private void onMainMenu()
    {
        if (listChessSelected.Count > 0)
        {
            listChessSelected.Clear();
        }

        score = 0;

        countselected = 0;

        Timing.KillCoroutines(timerHandle);
    }

    public void SetSelected(Vector3 _posSelection, int _idChess)
    {
        var chessSelected = createUIChessSelected(_posSelection, _idChess);
        var rectSlot = listSlot[countselected].GetComponent<RectTransform>();
        chessSelected.MoveToSlot(rectSlot.anchoredPosition);
        countselected++;
    }

    public void OnWinGame(int _levelNext)
    {
        txtScore.SetText("0");
        score = 0;
        countselected = 0;
        Timing.KillCoroutines(timerHandle);
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

    private void onStartCountTimer(int _timer)
    {
        txtScore.SetText("0");
        timerHandle = Timing.RunCoroutine(updateTimer(_timer));
    }

    private void onWinMatch(int _idMatch, int _score)
    {
        Timing.RunCoroutine(coroutineWinMatch(_idMatch, _score));
    }

    private IEnumerator<float> coroutineWinMatch(int _idMatch, int _score)
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
                countselected--;
                if (countselected < 0)
                {
                    countselected = 0;
                }
            }
        }

        yield return Timing.WaitForSeconds(0.2f);

        DOTween.To((score) => { txtScore.SetText(((int)score).ToString()); }, score, _score, 0.5f)
            .OnComplete(() => score = _score);


        if (listChessSelected.Count > 0)
        {
            for (var i = 0; i < listChessSelected.Count; i++)
            {
                var chess = listChessSelected[i];
                chess.SetIndexSlot(i);
                var rectSlot = listSlot[i].GetComponent<RectTransform>();
                chess.MoveToSlot(rectSlot.anchoredPosition);
            }
        }

        EventHandle.OnCheckMatchDone.Invoke();
    }

    private IEnumerator<float> updateTimer(int _timer)
    {
        var totalTime = _timer * 60;
        while (totalTime > 0)
        {
            int minutes = Mathf.FloorToInt(totalTime / 60);
            int seconds = Mathf.FloorToInt(totalTime % 60);

            txtTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            yield return Timing.WaitForSeconds(1);
            totalTime -= 1;
        }

        EventHandle.OnTimeOut.Invoke();
        txtTimer.SetText("0:00");
        Timing.KillCoroutines(timerHandle);
    }

    #endregion

    private void OnDestroy()
    {
        EventHandle.OnStarCountTimer -= onStartCountTimer;
        EventHandle.OnCheckMatchStart -= onWinMatch;
        EventHandle.OnMainMenu -= onMainMenu;
        btnMainMenu.onClick.RemoveListener(onClickMainMenu);
    }
}