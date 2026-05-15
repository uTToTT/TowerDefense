using UnityEngine;

public abstract class MapObject : MonoBehaviour, IPoolable, IEntityLifecycle
{
    [SerializeField] private MapObjectType _type;
    [SerializeField] private MapObjectShape _shape;

    public MapObjectType Type => _type;
    public Vector2Int MapPos { get; set; }
    public MapObjectShape Shape => _shape;

    public bool IsActive { get; set; }

    public virtual void Dispose() { }
    public virtual void OnActivated() { }
    public virtual void OnDeactivated() { }
    public virtual void OnDestroyed() { }
    public virtual void OnPreload() { }
    public virtual void OnReturned() { }
}
