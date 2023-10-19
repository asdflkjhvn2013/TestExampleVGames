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
    private Vector2 endPos;

    public void Initialize()
    {
        inputControl = new InputControl();
        _handle = new CoroutineHandle();
        endPos = new Vector2();

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

        _handle = Timing.RunCoroutine(updatePos(obj));
    }

    private IEnumerator<float> updatePos(InputAction.CallbackContext _context)
    {
        while (true)
        {
            if (isMouseDown)
            {
#if UNITY_ANDROID
                endPos = inputControl.Mouse.TouchPosition.ReadValue<Vector2>();
#endif

#if UNITY_EDITOR
                endPos = Mouse.current.position.ReadValue();
#endif

                onMouseDown?.Invoke(endPos);
            }

            yield return Timing.WaitForOneFrame;
        }
    }


    private void pressOnStarted(InputAction.CallbackContext obj)
    {
        isMouseDown = true;
#if UNITY_ANDROID
        endPos = inputControl.Mouse.TouchPosition.ReadValue<Vector2>();
#endif

#if UNITY_EDITOR
        endPos = Mouse.current.position.ReadValue();
#endif
    }

    private void pressOnCanceled(InputAction.CallbackContext obj)
    {
        isMouseDown = false;

        Timing.KillCoroutines(_handle);

        onMouseUp?.Invoke(endPos);
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