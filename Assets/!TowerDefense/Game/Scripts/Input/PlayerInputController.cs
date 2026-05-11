using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputController
{
    public event Action OnTapPerformed;
    public event Action OnTapCanceled;

    public event Action<Vector2> OnTap;
    public event Action OnTapReleased;

    private readonly Camera _camera;
    private readonly InputActions _input;

    public bool IsPointerDown { get; private set; }

    public PlayerInputController(Camera camera)
    {
        _camera = camera;
        _input = new InputActions();
        _input.Enable();

        _input.GamePlay.Tap.performed += TapPerformed;
        _input.GamePlay.Tap.canceled += TapCanceled;
    }

    private void TapPerformed(InputAction.CallbackContext context)
    {
        IsPointerDown = true;
        OnTapPerformed?.Invoke();

        if (!IsPointerOverUI())
            OnTap?.Invoke(GetPointerPosition());
    }

    private void TapCanceled(InputAction.CallbackContext context)
    {
        IsPointerDown = false;
        OnTapCanceled?.Invoke();
        OnTapReleased?.Invoke(); 
    }

    public Vector2 GetPointerPosition()
    {
        Vector3 screenPos = _input.GamePlay.PointerPos.ReadValue<Vector2>();
        screenPos.z = -_camera.transform.position.z;

        Vector2 worldPos = _camera.ScreenToWorldPoint(screenPos);
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
