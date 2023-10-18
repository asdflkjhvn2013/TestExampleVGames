using System;
using System.Collections.Generic;
using MEC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIResult : MonoBehaviour, IGuiItem
{
    [SerializeField] private TextMeshProUGUI txtLevel;
    [SerializeField] private Button btnNext;
    [SerializeField] private Button btnMenu;
    [SerializeField] private Button btnTryAgain;
    [SerializeField] private GameObject guiWin;
    [SerializeField] private GameObject guiLose;

    private GameObject currGui;

    public void Init(int _level)
    {
        btnMenu.onClick.AddListener(onMenu);
        btnNext.onClick.AddListener(onNextLevel);
        btnTryAgain.onClick.AddListener(onTryAgain);

        EventHandle.OnTimeOut += onTimeOut;
    }

    private void onTimeOut()
    {
        Timing.RunCoroutine(openUILose());
    }

    private IEnumerator<float> openUILose()
    {
        yield return Timing.WaitForSeconds(1f);
        btnMenu.gameObject.SetActive(true);
        guiLose.SetActive(true);
        currGui = guiLose;
    }


    public void SetSelected(Vector3 _posSelection, int _idChess)
    {
    }

    public void OnWinGame(int _levelNext)
    {
        Timing.RunCoroutine(openUIWin(_levelNext));
    }

    private IEnumerator<float> openUIWin(int _levelNext)
    {
        yield return Timing.WaitForSeconds(1);
        
        txtLevel.SetText(_levelNext.ToString());
        btnMenu.gameObject.SetActive(true);
        guiWin.SetActive(true);
        currGui = guiWin;
    }

    private void onNextLevel()
    {
        guiWin.SetActive(false);
        btnMenu.gameObject.SetActive(false);
        EventHandle.OnNextLevel.Invoke();
    }

    private void onTryAgain()
    {
        guiLose.SetActive(false);
        btnMenu.gameObject.SetActive(false);
        EventHandle.OnNextLevel.Invoke();
    }

    private void onMenu()
    {
        currGui?.SetActive(false);
        btnMenu.gameObject.SetActive(false);
        EventHandle.OnMainMenu.Invoke();
    }

    private void OnDestroy()
    {
        btnMenu.onClick.RemoveListener(onMenu);
        btnNext.onClick.RemoveListener(onNextLevel);
        btnTryAgain.onClick.RemoveListener(onTryAgain);
    }
}