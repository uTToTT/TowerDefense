using NaughtyAttributes;
using UnityEngine;

public class TowerPreview : MonoBehaviour, IPoolable, IEntityLifecycle
{
    [SerializeField] private TowerType _towerType;
    [SerializeField] private TowerShapeSO _towerShape;
    [Expandable]
    [SerializeField] private TargetingModuleConfig _config;
    [HorizontalLine]

    [SerializeField] private GameObject _minRange;
    [SerializeField] private GameObject _maxRange;

    public TowerType TowerType => _towerType;
    public TowerShapeSO Shape => _towerShape;
    public bool IsActive { get; set; }

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

    public void Dispose() { }
    public void OnActivated() { }
    public void OnDeactivated() { }
    public void OnDestroyed() { }
    public void OnPreload() { }
    public void OnReturned() { }
}
