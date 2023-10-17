using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    private IOutlineHandle _currOutlineHandle;
    
    public void SetOutline(GameObject _target)
    {
        _currOutlineHandle?.UnOutline();
        _currOutlineHandle = _target.GetComponent<IOutlineHandle>();
        if (_currOutlineHandle != null)
        {
            _currOutlineHandle.SetOutline();
        }
    }

    public void UnOutline()
    {
        _currOutlineHandle?.UnOutline();
    }
}