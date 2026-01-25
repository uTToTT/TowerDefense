using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputController
{
    public event Action<Vector2> OnTap;

    private InputActions _input;

    public void Init()
    {
        _input = new InputActions();
        _input.Enable();

        _input.GamePlay.Tap.performed += OnTapPerformed;
    }

    private void OnTapPerformed(InputAction.CallbackContext context)
    {
        if (IsPointerOverUI()) return;

        var v2 = _input.GamePlay.PointerPos.ReadValue<Vector2>();
        OnTap?.Invoke(v2);
        Debug.Log($"Tap | {v2}");
    }

    public void EnableInput()
    {
        _input.GamePlay.Enable();
    }

    public void DisableInput()
    {
        _input.GamePlay.Disable();
    }

    private bool IsPointerOverUI()
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
