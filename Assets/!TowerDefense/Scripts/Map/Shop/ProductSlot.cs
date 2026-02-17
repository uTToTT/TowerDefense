using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProductSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action<ProductSlot> OnDragPerformed;
    public event Action<ProductSlot> OnDragCanceled;

    [SerializeField] private TMP_Text _price;

    public MapObject MapObject;

    public void SetProduct(MapObject mapObject, ProductConfig product)
    {
        if (mapObject == null) return;

        MapObject = mapObject;

        PositionMapObject();

        _price.text = product.Cost.ToString();
    }

    public void Clear() => MapObject = null;

    private void PositionMapObject()
    {
        MapObject.transform.position = transform.position;

        MapObject.SetRenderLayer(RenderLayers.UI);
        MapObject.SetRenderOrder(100);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDragPerformed?.Invoke(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnDragCanceled?.Invoke(this);
    }
}
