public class BuildManager 
{
    private readonly MapObjectFactoryRegistry _factory;

    public BuildManager(MapObjectFactoryRegistry factoryRegistry)
    {
        _factory = factoryRegistry;
        _factory.Init();
    }

    public MapObject Create(MapObjectType type) =>
        _factory.Create(type);

    public void Return(MapObject mapObject) =>
        _factory.Return(mapObject);
}
