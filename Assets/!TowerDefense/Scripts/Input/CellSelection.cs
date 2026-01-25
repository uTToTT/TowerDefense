using UnityEngine;

public class CellSelection : MonoBehaviour, IPoolable, IEntityLifecycle
{
    public bool IsActive { get; set; }

    public void Dispose()
    {
    }

    public void OnActivated()
    {
    }

    public void OnDeactivated()
    {
    }

    public void OnDestroyed()
    {
    }

    public void OnPreload()
    {
    }

    public void OnReturned()
    {
    }
}
