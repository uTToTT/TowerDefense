using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProductSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action<ProductSlot> OnDragPerformed;
    public event Action<ProductSlot> OnDragCanceled;

    [SerializeField] private TMP_Text _price;
    [SerializeField] private Transform _productContainer;

    public bool IsEmpty => MapObject == null;

    public MapObject MapObject;
    public ProductConfig ProductConfig;

    public void SetProduct(MapObject mapObject, ProductConfig product)
    {
        if (mapObject == null) return;

        MapObject = mapObject;
        ProductConfig = product;

        PositionMapObject();

        _price.text = product.Cost.ToString();
    }

    public void Clear()
    {
        MapObject = null;
        ProductConfig = null;
    }

    private void PositionMapObject()
    {
        MapObject.transform.position = _productContainer.transform.position;
        MapObject.transform.parent = _productContainer.transform;

        MapObject.SetRenderLayer(RenderLayers.UI);
        MapObject.SetRenderOrder(100);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsEmpty) return;

        OnDragPerformed?.Invoke(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsEmpty) return;

        OnDragCanceled?.Invoke(this);
    }
}
