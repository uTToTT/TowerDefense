using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProductSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
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
        MapManager.Instance.MapObjectDragger.BeginDrag(MapObject);
        Debug.Log("Down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        MapManager.Instance.MapObjectDragger.EndDrag();
        Debug.Log("UP");
    }
}
