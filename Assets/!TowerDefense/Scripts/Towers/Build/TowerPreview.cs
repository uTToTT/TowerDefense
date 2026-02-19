using NaughtyAttributes;
using UnityEngine;

public class TowerPreview : MapObject, IPoolable, IEntityLifecycle
{
    [Expandable]
    [SerializeField] private TargetingModuleConfig _config;
    [HorizontalLine]

    [SerializeField] private GameObject _minRange;
    [SerializeField] private GameObject _maxRange;

    [Button]
    private void UpdateRange()
    {
        if (_config == null ||
            _minRange == null ||
            _maxRange == null) return;

        _minRange.transform.localScale =
            new Vector3(_config.MinRange * 2, _config.MinRange * 2);
        _maxRange.transform.localScale =
            new Vector3(_config.MaxRange * 2, _config.MaxRange * 2);
    }

    public void Enable()
    {
        EnableRangePreview();
    }

    public void Disable()
    {
        DisableRangePreview();
    }

    public override void Dispose() { }
    public override void OnActivated() { }
    public override void OnDeactivated() { }
    public override void OnDestroyed() { }
    public override void OnPreload() { }
    public override void OnReturned() { }

    private void DisableRangePreview()
    {
        _minRange.SetActive(false);
        _maxRange.SetActive(false);
    }

    private void EnableRangePreview()
    {
        _minRange.SetActive(true);
        _maxRange.SetActive(true);
    }
}
