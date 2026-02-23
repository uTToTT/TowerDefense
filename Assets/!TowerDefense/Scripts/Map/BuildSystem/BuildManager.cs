using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private MapObjectFactoryRegistry _factory;

    public void Init()
    {
        _factory.Init();
    }

    public MapObject Create(MapObjectType type) =>
        _factory.Create(type);

    public void Return(MapObject mapObject) =>
        _factory.Return(mapObject);
}
