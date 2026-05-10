using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TToTT.TowerDefense.UI.Label
{
    public class LabelRegistry : MonoBehaviour, IDisposable
    {
        [SerializeField] private LabelEntry[] _entries;

        [Serializable]
        private class LabelEntry
        {
            public LabelId Id;
            public LabelView View;
        }

        private Dictionary<LabelId, ILabelView> _map;
        private bool _initialized = false;

        public void Init()
        {
            _map = _entries.ToDictionary(e => e.Id, e => (ILabelView)e.View);
        }

        public ILabelView Get(LabelId id)
        {
            if (!_initialized) Init();
            return _map[id];
        }

        public void Dispose()
        {
            _map.Clear();
        }
    }
}