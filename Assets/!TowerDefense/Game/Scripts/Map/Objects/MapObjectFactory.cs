using UnityEngine;

[CreateAssetMenu(fileName = "MapObjectFactory", menuName = "TD/Map/Map Object Factory")]
public class MapObjectFactory : FactoryBase<MapObject>
{
    public MapObjectType Type => Prefab.Type;
}
