using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class GUIChessSelectedItem : MonoBehaviour
{
    [SerializeField] private Image imageChess;
    [SerializeField] private float duration;
    [SerializeField] private float scale;

    private int idChess;
    private int indexSlot;

    public void Init(Vector3 _pos, Sprite _sprite, int _indexSlot,int _idChess)
    {
        transform.position = _pos;
        imageChess.sprite = _sprite;
        indexSlot = _indexSlot;
        idChess = _idChess;
    }

    public void MoveToSlot(Vector3 _pos, Action _callback = null)
    {
        var rect = GetComponent<RectTransform>();
        rect.DOAnchorPos(_pos, duration).OnComplete(() =>
        {
            _callback?.Invoke();
        });
    }

    public int GetIdChess()
    {
        return idChess;
    }
    public int GetIndexSlot()
    {
        return indexSlot;
    }

    public void ScaleWinMatch()
    {
        transform.DOScale(scale, duration).OnComplete(() =>
        {
            transform.DOScale(scale - 0.3f, duration);
        });
    }

    public void SetIndexSlot(int _index)
    {
        indexSlot = _index;
    }
}