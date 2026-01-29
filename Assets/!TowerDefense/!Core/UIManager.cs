using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<WindowData> _windowDatas = new();

    public static UIManager Instance { get; private set; }

    public void Init()
    {
        Instance = this;
    }

    public void OpenWindow(WindowType windowType) =>
        FindWindowData(windowType).Window.SetActive(true);
    public void CloseWindow(WindowType windowType) =>
        FindWindowData(windowType).Window.SetActive(false);

    public void CloseAllWindows()
    {
        foreach (var data in _windowDatas)
        {
            data.Window.SetActive(false);
        }
    }

    private WindowData FindWindowData(WindowType type) => _windowDatas.FirstOrDefault(w => w.Type == type);
}
