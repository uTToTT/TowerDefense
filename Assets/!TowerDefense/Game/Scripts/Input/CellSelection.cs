using UnityEngine;

public class CellSelection : MonoBehaviour, IPoolable, IEntityLifecycle
{
    [SerializeField] private Color _isFreeColorBackground;
    [SerializeField] private Color _isBusyColorBackground;
    [Space]
    [SerializeField] private Color _isFreeColorBorder;
    [SerializeField] private Color _isBusyColorBorder;
    [Space]
    [Space]
    [SerializeField] private SpriteRenderer _border;
    [SerializeField] private SpriteRenderer _background;

    public bool IsActive { get; set; }

    public void SetFreeColor()
    {
        if (_border != null)
            _border.color = _isFreeColorBorder;
        if (_background != null)
            _background.color = _isFreeColorBackground;
    }

    public void SetBusyColor()
    {
        if (_border != null)
            _border.color = _isBusyColorBorder;
        if (_background != null)
            _background.color = _isBusyColorBackground;
    }

    public void Dispose() { }
    public void OnActivated() { }
    public void OnDeactivated() { }
    public void OnDestroyed() { }
    public void OnPreload() { }
    public void OnReturned() { }
}
