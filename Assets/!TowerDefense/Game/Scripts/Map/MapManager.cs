/// <summary> 
/// 
/// TODO:
/// - MapLoader DONE
/// - MapDebugger
/// 
/// </summary>

namespace TToTT.TowerDefense.Map
{
    public class MapManager
    {
        private readonly MapController _controller;
        private readonly MapLoader _loader;
        private readonly MapComposer _composer;

        #region Init

        public MapManager(
            MapController mapController,
            MapLoader mapLoader,
            MapComposer mapComposer)
        {
            _controller = mapController;
            _loader = mapLoader;
            _composer = mapComposer;

            // TEMP
#if UNITY_EDITOR
            TryBuildMap(0);
#endif
        }

        #endregion

        public bool TryBuildMap(int index)
        {
            if (!_loader.TryLoad(index, out var mapData))
            {
                return false;
            }

            _controller.SetMap(mapData);
            _composer.Build(mapData);
            _controller.InitCellStates(mapData);

            return true;
        }
    }
}