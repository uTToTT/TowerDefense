using UnityEngine;
using UnityEngine.EventSystems;

public class TowerBuildButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TowerType _towerType;

    public TowerManager TowerPlacer { get; set; }
    public TowerType TowerType => _towerType;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (TowerPlacer != null)
            TowerPlacer.BeginDrag(_towerType);
    }
}
