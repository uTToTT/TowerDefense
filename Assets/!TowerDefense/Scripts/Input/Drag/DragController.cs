using System;
using UnityEngine;

public sealed class DragController
{
    public event Action<DragContext> OnBeginDrag;
    public event Action<DragContext> OnEndDrag;

    private bool _isDragging;
    private bool _enabled;

    private DragContext _dragContext;

    private Vector3 GetPointerPosition() => 
        GameManager.Instance.PlayerInputController.GetPointerPosition();

    public void Tick(float dt)
    {
        if (!_enabled) return;
        if (!_isDragging) return;

        _dragContext.PointerPosition = GetPointerPosition();

        if (_dragContext.MapObject != null)
            DragUtils.SnapToPointer(_dragContext.MapObject.transform);
    }

    public void EnableDrag() => _enabled = true;
    public void DisableDrag() => _enabled = false;

    public void BeginDrag(DragContext ctx)
    {
        _dragContext.PointerDownPosition = GetPointerPosition();
        _dragContext.MapObject = ctx.MapObject;

        _isDragging = true;
    }

    public void EndDrag(DragContext ctx)
    {
        _dragContext.PointerUpPosition = ctx.PointerUpPosition;

        _isDragging = false;
    }
}
