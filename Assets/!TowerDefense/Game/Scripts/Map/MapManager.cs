
using System;

/// <summary> 
/// 
/// TODO:
/// - MapLoader DONE
/// - MapDebugger
/// 
/// </summary>
namespace TToTT.TowerDefense.Map
{
    public class MapManager : IDisposable, ITickable
    {
        private readonly PlacementController _placementController;
        private readonly SellectionController _objectSelector;
        private readonly MapController _controller;
        private readonly MapLoader _loader;
        private readonly MapComposer _composer;

        #region Init

        public MapManager(
            MapController mapController,
            MapLoader mapLoader,
            MapComposer mapComposer,
            SellectionController objectSelector,
            PlacementController placementController)
        {
            _controller = mapController;
            _loader = mapLoader;
            _composer = mapComposer;
            _objectSelector = objectSelector;
            _placementController = placementController;

            // TEMP
#if UNITY_EDITOR
            TryBuildMap(0);
#endif
        }

        public void Dispose()
        {
            _controller.Dispose();
            _composer.Dispose();
            _placementController.Dispose();
        }

        #endregion

        #region Game loop

        public void Tick(float dt)
        {
            _placementController.Tick(dt);
            _objectSelector.Tick(dt);
        }

        public void Restart()
        {
            _composer.Release();
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