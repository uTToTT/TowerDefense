using UnityEngine;

public abstract class MapObject : MonoBehaviour, IPoolable, IEntityLifecycle
{
    [SerializeField] private MapObjectType _type;
    [SerializeField] private MapObjectShape _shape;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public MapObjectType Type => _type;
    public Vector2Int MapPos { get; set; }
    public MapObjectShape Shape => _shape;

    public bool IsActive { get; set; }

    public void SetRenderOrder(int order) => _spriteRenderer.sortingOrder = order;
    public void SetRenderLayer(string layerName) => _spriteRenderer.sortingLayerName = layerName;
    public void SetPosition(Vector2Int pos)
    {
        transform.position = new Vector3(pos.x, pos.y, 0);
        MapPos = pos;
    }

    public virtual void Dispose() { }
    public virtual void OnActivated() { }
    public virtual void OnDeactivated() { }
    public virtual void OnDestroyed() { }
    public virtual void OnPreload() { }
    public virtual void OnReturned() { }
}
