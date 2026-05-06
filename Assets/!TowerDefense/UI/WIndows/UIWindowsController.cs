using System.Collections.Generic;
using UnityEngine;

namespace TToTT.TowerDefense.UI
{
    public class UIWindowsController : MonoBehaviour
    {
        [SerializeField] private List<WindowData> _windowDatas = new();

        private Dictionary<WindowType, WindowData> _windowMap = new();

        public void Init()
        {
            foreach (var data in _windowDatas)
            {
                _windowMap.Add(data.Type, data);
            }
        }

        public void OpenWindow(WindowType type) => SetWindowState(type, true);
        public void CloseWindow(WindowType type) => SetWindowState(type, false);

        private void SetWindowState(WindowType type, bool state)
        {
            if (!_windowMap.ContainsKey(type))
            {
#if UNITY_EDITOR
                Debug.Log($"Not found window [{type}]");
#endif
                return;
            }

            foreach (var e in _windowMap[type].Elements)
                e.SetActive(state);
        }
    }
}