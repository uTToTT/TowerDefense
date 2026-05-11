using System;
using System.Collections.Generic;
using TToTT.TowerDefense.Gameloop;
using TToTT.TowerDefense.Map;

namespace TToTT.TowerDefense.Towers
{
    public class TowerManager : IDisposable, ITickable
    {
        private readonly List<Tower> _builtTowers = new();
        private readonly List<Tower> _toAdd = new();
        private readonly List<Tower> _toRemove = new();

        private readonly BuildController _buildController;
        private readonly GameStateMachine _gameState;
        public List<Tower> Towers => _builtTowers;

        #region Init

        public TowerManager(
            BuildController buildController,
            GameStateMachine gameState)
        {
            _buildController = buildController;
            _gameState = gameState;
        }

        #endregion


        #region Life cycle

        public void Restart()
        {
            UpdateColleciton();

            foreach (var tower in _builtTowers)
            {
                _buildController.TearDown(tower);
            }

            _builtTowers.Clear();
            _toAdd.Clear();
            _toRemove.Clear();
        }

        public void Tick(float dt)
        {
            if (_gameState.State != GameState.Wave &&
               _gameState.State != GameState.Preparing) return;

            UpdateColleciton();

            foreach (var tower in _builtTowers)
                tower.Tick(dt);
        }

        #endregion

        #region Registering

        public void Register(Tower tower) => _toAdd.Add(tower);
        public void Unregister(Tower tower) => _toRemove.Add(tower);

        #endregion

        private void UpdateColleciton()
        {
            if (_toRemove.Count > 0)
            {
                foreach (var item in _toRemove)
                    _builtTowers.Remove(item);

                _toRemove.Clear();
            }

            if (_toAdd.Count > 0)
            {
                foreach (var item in _toAdd)
                    _builtTowers.Add(item);

                _toAdd.Clear();
            }
        }

        public void Dispose()
        {
            _builtTowers.Clear();
            _toAdd.Clear();
            _toRemove.Clear();
        }
    }
}