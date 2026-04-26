/// <summary> 
/// 
/// TODO:
/// - MapLoader
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
        }

        #endregion

        public void BuildMap(int index)
        {
            var mapData = _loader.Load(index);
            _controller.SetMap(mapData);
            _composer.Build(mapData);
            _controller.InitCellStates(mapData);
        }
    }
}