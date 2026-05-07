using TToTT.TowerDefense.Map;
using UnityEngine;

namespace TToTT.TowerDefense.Installers
{
    public class MapContext : MonoBehaviour
    {
        [SerializeField] public Grid Grid;
        [SerializeField] public CellFactoryRegistry CellFactory;
        [SerializeField] public MapObjectFactoryRegistry ObjectFactory;
        [SerializeField] public MapObjectPreviewFactoryRegistry PreviewFactory;
        [SerializeField] public MapRegistry Maps;
        [SerializeField] public CellSelectionFactory SelectionFactory;
    }
}