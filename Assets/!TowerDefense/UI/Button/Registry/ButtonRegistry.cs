using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TToTT.TowerDefense.UI.Button
{
    public class ButtonRegistry : MonoBehaviour,IDisposable
    {
        [SerializeField] private ButtonEntry[] _entries;

        [Serializable]
        private class ButtonEntry
        {
            public ButtonId Id;
            public ButtonWrapper View;
        }

        private Dictionary<ButtonId, IUIButton> _map;
        private bool _initialized = false;

        public void Init()
        {
            _map = _entries.ToDictionary(e => e.Id, e => (IUIButton)e.View);
        }

        public IUIButton Get(ButtonId id)
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