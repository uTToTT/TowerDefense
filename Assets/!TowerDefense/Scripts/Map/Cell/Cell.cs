using UnityEngine;

public class Cell : MonoBehaviour, IPoolable, IEntityLifecycle
{
    [SerializeField] private CellType _cellType;

    private GameObject _tower;

    public CellType CellType => _cellType;

    public bool IsActive { get; set; }

    private void OnValidate()
    {
        
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
        //EventBus.AddMoney?.Invoke(_tower.GetComponent<Tower>().CurrSellCost);
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
        //Debug.Log("Activated");
    }

    public void OnDeactivated()
    {
    }

    public void OnReturned()
    {
        //Debug.Log("OnReturned");
    }

    public void OnDestroyed()
    {
    }
}

