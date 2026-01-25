using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputController
{
    public event Action OnTapPerformed;
    public event Action OnTapCanceled;

    private InputActions _input;

    public bool IsPointerDown { get; private set; }

    public void Init()
    {
        _input = new InputActions();
        _input.Enable();

        _input.GamePlay.Tap.performed += TapPerformed;
        _input.GamePlay.Tap.canceled += TapCanceled;
    }

    private void TapPerformed(InputAction.CallbackContext context)
    {
        OnTapPerformed?.Invoke();
        IsPointerDown = true;
    }

    private void TapCanceled(InputAction.CallbackContext context)
    {
        OnTapCanceled?.Invoke();
        IsPointerDown = false;
    }

    public Vector2 GetPointerPosition() => 
        _input.GamePlay.PointerPos.ReadValue<Vector2>();

    public void EnableInput()
    {
        _input.GamePlay.Enable();
    }

    public void DisableInput()
    {
        _input.GamePlay.Disable();
    }

    public bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
            return false;

        // Touch
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.isPressed)
        {
            return EventSystem.current.IsPointerOverGameObject(
                Touchscreen.current.primaryTouch.touchId.ReadValue()
            );
        }

        // Mouse
        return EventSystem.current.IsPointerOverGameObject();
    }
}
