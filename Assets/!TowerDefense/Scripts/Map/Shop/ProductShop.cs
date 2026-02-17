using UnityEngine;
using UnityEngine.UI;

public class ProductShop : MonoBehaviour
{
    [SerializeField] private ProductSlot[] _slots;
    [SerializeField] private ProductConfig[] _products;
    [SerializeField] private Button _reroll;
    [SerializeField] private MapObjectPreviewFactoryRegistry _previewFactory;

    private MapObject _selectedProduct;
    private ProductSlot _selectedSlot;

    private PlacementController PlacementController => MapManager.Instance.PlacementController;

    public void Init()
    {
        _previewFactory.Init();
        _reroll.onClick.AddListener(() => Reroll());

        foreach (var s in _slots)
        {
            s.OnDragPerformed += OnProductDragPerformed;
            s.OnDragCanceled += OnProductDragCanceled;
        }
    }

    public void Reroll()
    {
        ClearSlots();

        float totalWeight = CalculateTotalWeight();

        for (int si = 0; si < _slots.Length; si++)
        {
            var product = PickWeighted(totalWeight);

            var mapObject = _previewFactory.Create(product.ProductType);
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
        PlacementController.BeginDrag(slot.MapObject);
        _selectedProduct = slot.MapObject;

        PlacementController.OnPlaced += OnProductPlacePerformed;
        PlacementController.OnCanceled += OnProductPlaceCanceled;
    }

    private void OnProductDragCanceled(ProductSlot slot)
    {
        PlacementController.EndDrag(slot.MapObject);
        _selectedProduct = null;

        PlacementController.OnPlaced -= OnProductPlacePerformed;
        PlacementController.OnCanceled -= OnProductPlaceCanceled;
    }

    private void OnProductPlacePerformed()
    {
        _previewFactory.Return(_selectedProduct);
        var obj = GameManager.Instance.BuildManager.Create(_selectedProduct.Type);
        obj.transform.position = 
            MapUtils.SnapToGrid(_selectedProduct.transform.position, MapManager.Instance.Grid);
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
