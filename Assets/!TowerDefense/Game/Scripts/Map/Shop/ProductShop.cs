using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProductShop : MonoBehaviour
{
    [SerializeField] private ProductSlot[] _slots;
    [SerializeField] private ProductConfig[] _products;
    [SerializeField] private ButtonWrapper _reroll;
    [SerializeField] private MapObjectPreviewFactoryRegistry _previewFactory;

    [Header("Reroll economy")]
    [SerializeField] private int _startRerollCost = 5;
    [SerializeField] private int _rerollDelta;

    private MapObject _selectedProduct;
    private ProductSlot _selectedSlot;

    private float _totalWeight;
    private int _currRerollCost;

    private PlacementController PlacementController => MapManager.Instance.PlacementController;

    public void Init()
    {
        _previewFactory.Init();
        _reroll.OnClick += Reroll;
        _totalWeight = CalculateTotalWeight();

        Restart();

        foreach (var s in _slots)
        {
            s.OnDragPerformed += OnProductDragPerformed;
            s.OnDragCanceled += OnProductDragCanceled;
        }
    }

    public void Restart()
    {
        _currRerollCost = _startRerollCost;
        Reroll(true);
    }

    public void Reroll()
    {
        Reroll(false);
    }

    public void Reroll(bool free = true)
    {
        if (!free)
        {
            if (!EconomyManager.Instance.CanSpend(_currRerollCost))
            {
                Debug.Log("Not enough money!");
                return;
            }

            EconomyManager.Instance.Spend(_currRerollCost);
            _currRerollCost += _rerollDelta;
        }

        ClearSlots();

        for (int si = 0; si < _slots.Length; si++)
        {
            var product = PickWeighted(_totalWeight);

            var mapObject = _previewFactory.Create(product.ProductType);
            if (mapObject is TowerPreview towerPrev)
            {
                towerPrev.Disable();
            }
            _slots[si].SetProduct(mapObject, product);
        }
    }

    public void ClearSlots()
    {
        foreach (var slot in _slots)
        {
            if (slot.MapObject != null)
            {
                _previewFactory.Return(slot.MapObject);
                slot.Clear();
            }
        }
    }

    #region Handlers

    private void OnProductDragPerformed(ProductSlot slot)
    {
        if (!EconomyManager.Instance.CanSpend(slot.ProductConfig.Cost))
        {
            Debug.Log("Not enough money!");
            return;
        }

        PlacementController.BeginDrag(slot.MapObject);
        _selectedProduct = slot.MapObject;
        _selectedSlot = slot;

        if (_selectedProduct is TowerPreview towerPrev)
        {
            towerPrev.Enable();
        }

        PlacementController.OnPlaced += OnProductPlacePerformed;
        PlacementController.OnCanceled += OnProductPlaceCanceled;
    }

    private void OnProductDragCanceled(ProductSlot slot)
    {
        PlacementController.EndDrag(slot.MapObject);

        if (_selectedProduct is TowerPreview towerPrev)
        {
            towerPrev.Disable();
        }

        _selectedProduct = null;
        _selectedSlot = null;

        PlacementController.OnPlaced -= OnProductPlacePerformed;
        PlacementController.OnCanceled -= OnProductPlaceCanceled;
    }

    private void OnProductPlacePerformed()
    {
        _previewFactory.Return(_selectedProduct);
        var grid = MapManager.Instance.Grid;
        var worldPos = _selectedProduct.transform.position;
        var obj = GameManager.Instance.BuildManager.Create(_selectedProduct.Type);
        var mapPos = MapUtils.WorldToMap(worldPos, grid);
        obj.transform.position = MapUtils.SnapToGrid(worldPos, grid);
        obj.MapPos = mapPos;

        if (obj is Tower tower)
        {
            tower.HideRange();
            tower.Enable();
            GameManager.Instance.TowerManager.Register(tower);
        }

        MapManager.Instance.PlaceMapObject(mapPos, obj);

        EconomyManager.Instance.Spend(_selectedSlot.ProductConfig.Cost);

        _selectedSlot.Clear();

        _selectedProduct = null;
        _selectedSlot = null;

        bool anySlotNotEmpty = _slots.Any(s => !s.IsEmpty);

        if (!anySlotNotEmpty)
        {
            Reroll();
        }
    }

    private void OnProductPlaceCanceled()
    {
        _selectedProduct.transform.position = _selectedSlot.transform.position;
    }

    #endregion

    #region Weights

    private float CalculateTotalWeight()
    {
        float total = 0f;

        for (int i = 0; i < _products.Length; i++)
            total += _products[i].Weight;

        return total;
    }

    private ProductConfig PickWeighted(float totalWeight)
    {
        float r = Random.value * totalWeight;
        float cumulative = 0f;

        for (int i = 0; i < _products.Length; i++)
        {
            cumulative += _products[i].Weight;

            if (r <= cumulative)
                return _products[i];
        }

        return _products[_products.Length - 1];
    }

    #endregion
}
