using System;
using TToTT.TowerDefense.Gameloop;
using TToTT.TowerDefense.Level;

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
        private readonly GameStateMachine _gameState;

        #region Init

        public MapManager(
            MapController mapController,
            MapLoader mapLoader,
            MapComposer mapComposer,
            SellectionController objectSelector,
            PlacementController placementController,
            GameStateMachine gameState)
        {
            _controller = mapController;
            _loader = mapLoader;
            _composer = mapComposer;
            _objectSelector = objectSelector;
            _placementController = placementController;
            _gameState = gameState;
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
            if (_gameState.State != GameState.Preparing) return;

            _placementController.Tick(dt);
            _objectSelector.Tick(dt);
        }

        public void Restart()
        {
            _composer.Release();
        }

        #endregion

        public bool TryBuildMap(LevelData level)
        {
            if (!_loader.TryLoad(level, out var mapData))
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