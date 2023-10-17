using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputManager : MonoBehaviour, IActionHandle
{
    private InputControl inputControl;
    private bool isMouseDown;
    private Action<Vector2> onMouseDown;
    private Action<Vector2> onMouseUp;
    private CoroutineHandle _handle;

    public void Initialize()
    {
        inputControl = new InputControl();
        _handle = new CoroutineHandle();

        inputControl.Mouse.Press.started += pressOnStarted;
        inputControl.Mouse.Press.performed += PressOnperformed;
        inputControl.Mouse.Press.canceled += pressOnCanceled;

        inputControl.Enable();
    }

    private void PressOnperformed(InputAction.CallbackContext obj)
    {
        if (_handle != null)
        {
            Timing.KillCoroutines(_handle);
        }
        _handle =Timing.RunCoroutine(updatePos(obj));
    }

    private IEnumerator<float> updatePos(InputAction.CallbackContext _context)
    {
        while (true)
        {
            if (isMouseDown)
            {
                var _pos = Mouse.current.position.ReadValue();
                onMouseDown?.Invoke(_pos);
            }
            yield return Timing.WaitForOneFrame;
        }
    }


    private void pressOnStarted(InputAction.CallbackContext obj)
    {
        isMouseDown = true;
    }

    private void pressOnCanceled(InputAction.CallbackContext obj)
    {
        isMouseDown = false;
        Timing.KillCoroutines(_handle);

        var _pos = Mouse.current.position.ReadValue();
        onMouseUp?.Invoke(_pos);
    }

    public void AssignEvent(Action<Vector2> _onMouseDown, Action<Vector2> _onMouseUp)
    {
        onMouseDown = _onMouseDown;
        onMouseUp = _onMouseUp;
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void OnDestroy()
    {
        inputControl.Mouse.Press.started -= pressOnStarted;
        inputControl.Mouse.Press.canceled -= pressOnCanceled;
    }
}