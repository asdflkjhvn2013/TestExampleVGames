using System;
using UnityEngine;

public class ChessPieces : MonoBehaviour, IChessItem,IOutlineHandle,ISelectionHandle
{
   [SerializeField] private int idChess;
    private Outline outline;

    public void SetOutline()
    {
        if (outline == null)
        {
            outline = gameObject.GetComponent<Outline>();
        }

        outline.enabled = true;
    }

    public void UnOutline()
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    public void SetSelected(Action<Vector3, int> _callback)
    {
        gameObject.SetActive(false);
        _callback.Invoke(transform.position,idChess);
    }

    public void AssignData(int _id)
    {
        idChess = _id;
    }
}