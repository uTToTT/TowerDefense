using UnityEngine;

[CreateAssetMenu(fileName = "MapObjectPreviewFactory", menuName = "TD/Map/Objects/Preview Factory")]

public class MapObjectPreviewFactory : FactoryBase<MapObject>
{
    public MapObjectType Type => Prefab.Type;
}
