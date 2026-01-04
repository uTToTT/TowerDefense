using UnityEngine;

public class Cell : MonoBehaviour, IPoolable, IEntityLifecycle
{
    [SerializeField] private CellType _cellType;
    [Space]
    [SerializeField] private Waypoint _waypointA;
    [SerializeField] private Waypoint _waypointB;
    [SerializeField] private Waypoint _waypointCentral;
    [Space]
    [SerializeField] private Direction _nextDirection;
    [Space]
    [SerializeField] private Direction _nextDirectionA;
    [SerializeField] private Direction _nextDirectionB;
    [SerializeField] private Direction _nextDirectionCentral;

    private GameObject _tower;

    public CellType CellType => _cellType;

    public bool IsActive { get; set; }

    private void OnValidate()
    {
        if (_waypointA == null ||
            _waypointB == null ||
            _waypointCentral == null)
        {
            return;
        }

        if (_cellType == CellType.Path)
        {
            if (_nextDirection != Direction.None)
            {
                _waypointA.SetDirection(_nextDirection);
                _waypointB.SetDirection(_nextDirection);
                _waypointCentral.SetDirection(_nextDirection);
            }
            else
            {
                _waypointA.SetDirection(_nextDirectionA);
                _waypointB.SetDirection(_nextDirectionB);
                _waypointCentral.SetDirection(_nextDirectionCentral);
            }
        }
    }

    public void SetTower(GameObject tower)
    {
        if (_tower == null)
        {
            _tower = tower;
        }
        else
        {
            Debug.Log("Tower already placed!");
        }

        gameObject.tag = "Ground_with_tower";
    }

    public GameObject GetTowerGameObject()
    {
        return _tower;
    }

    public Tower GetTower()
    {
        return _tower.GetComponent<Tower>();
    }

    public void UnRegisterTower()
    {
        EventBus.AddMoney?.Invoke(_tower.GetComponent<Tower>().CurrSellCost);
        Destroy(_tower.gameObject);
        gameObject.tag = "Ground";
    }

    public void Dispose()
    {
        
    }

    public void OnPreload()
    {
    }

    public void OnActivated()
    {
        Debug.Log("Activated");
    }

    public void OnDeactivated()
    {
    }

    public void OnReturned()
    {
        Debug.Log("OnReturned");
    }

    public void OnDestroyed()
    {
    }
}

