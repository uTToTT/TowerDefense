using System;
using System.Collections.Generic;

namespace TToTT.TowerDefense.Towers
{
    public class TowerManager : IDisposable
    {
        private readonly List<Tower> _builtTowers = new();
        private readonly List<Tower> _toAdd = new();
        private readonly List<Tower> _toRemove = new();

        public List<Tower> Towers => _builtTowers;


        #region Life cycle

        public void Restart()
        {
            //MapManager.Instance.RemoveMapObject(tower); // MapManager must remove on restart
            //GameLoop.Instance.BuildManager.Return(tower); // BuildManager must return on restart
        }

        public void Tick(float dt)
        {
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