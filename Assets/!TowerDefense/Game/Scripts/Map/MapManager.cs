using UnityEngine;

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
            MapLoader mapLoader)
        {
            _controller = mapController;
            _loader = mapLoader;
        }

        #endregion

        #region GameLoop

        public void Tick(float dt)
        {
        }

        public void Restart()
        {

        }

        #endregion

        public void BuildMap(int index)
        {
            var mapData = _loader.Load(index);
            _controller.SetMap(mapData);
            _composer.Build(mapData);
        }
    }
}