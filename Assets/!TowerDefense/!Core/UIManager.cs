using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<WindowData> _windowDatas = new();
    [SerializeField] private MainMenuController _mainMenu;
    [SerializeField] private GameplayInterfaceController _gameplay;

    public static UIManager Instance { get; private set; }

    public void Init()
    {
        Instance = this;

        InitActions();

        _mainMenu.Init();
        _gameplay.Init();
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

    private void InitActions()
    {
        GameLoop.Instance.OnGameDefeat += OpenDefeat;
        GameLoop.Instance.OnGameVictory += OpenVictory;
        GameLoop.Instance.OnWaveEnded += OpenPreparing;
        GameLoop.Instance.OnWaveStarted += OpenWave;
    }

    #region Windows

    // Gameplay

    public void OpenGameplay()
    {
        CloseAllWindows();
        OpenWindow(WindowType.Gameplay);
    }

    public void OpenPreparing()
    {
        OpenGameplay();
        _gameplay.OpenPreparing();
    }

    public void OpenDefeat()
    {
        OpenGameplay();
        _gameplay.OpenDefeat();
    }

    public void OpenVictory()
    {
        OpenGameplay();
        _gameplay.OpenVictory();
    }

    public void OpenWave()
    {
        OpenGameplay();
        _gameplay.OpenWave();
    }


    //--------
    public void OpenMain()
    {
        CloseAllWindows();
        OpenWindow(WindowType.Main);
        //_mainMenu.OpenPlay();
    }

    

    #endregion

    private WindowData FindWindowData(WindowType type) =>
        _windowDatas.FirstOrDefault(w => w.Type == type);
}
