using UnityEngine;

public class Cell : MonoBehaviour
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

    private void OnValidate()
    {
        if (_cellType == CellType.PathEdge)
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
}

enum CellType
{
    Path,
    PathEdge,
    Ground,
    Block
}
