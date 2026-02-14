public class MapObjectFactory : FactoryBase<MapObject>
{
    public MapObjectType Type => Prefab.Type;
}
