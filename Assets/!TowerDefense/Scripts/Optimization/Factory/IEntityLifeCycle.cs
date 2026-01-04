public interface IEntityLifecycle
{
    void OnPreload();
    void OnActivated();
    void OnDeactivated();
    void OnReturned();
    void OnDestroyed();
}
