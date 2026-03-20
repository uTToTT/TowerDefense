using UnityEngine;

public static class DragUtils 
{
    public static void SnapToPointer(Transform transform)
    {
        var worldPos = GameManager.Instance.PlayerInputController.GetPointerPosition();
        transform.position = worldPos;
    }
}
