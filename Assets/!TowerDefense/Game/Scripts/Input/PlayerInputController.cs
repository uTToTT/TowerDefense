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

    public PlayerInputController()
    {
        _input = new InputActions();
        _input.Enable();

        _input.GamePlay.Tap.performed += TapPerformed;
        _input.GamePlay.Tap.canceled += TapCanceled;
    }

    private void TapPerformed(InputAction.CallbackContext context)
    {
        IsPointerDown = true;
        OnTapPerformed?.Invoke();
    }

    private void TapCanceled(InputAction.CallbackContext context)
    {
        OnTapCanceled?.Invoke();
        IsPointerDown = false;
    }

    public Vector2 GetPointerPosition()
    {
        Vector3 screenPos = _input.GamePlay.PointerPos.ReadValue<Vector2>();
        screenPos.z = -UnityEngine.Camera.main.transform.position.z;

        Vector2 worldPos = UnityEngine.Camera.main.ScreenToWorldPoint(screenPos);
        return worldPos;
    }

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
