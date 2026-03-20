using UnityEngine;

public class GameplayInterfaceController : InterfaceContorller
{
    [SerializeField] private ButtonWrapper _victoryButtonToMain;
    [SerializeField] private ButtonWrapper _defeatButtonToMain;

    #region Init

    public void Init()
    {
        InitButtons();
        InitActions();
    }

    #endregion

    private void InitButtons()
    {
        _victoryButtonToMain.OnClick += UIManager.Instance.OpenMain;
        _defeatButtonToMain.OnClick += UIManager.Instance.OpenMain;
    }

    private void InitActions()
    {
      
    }

    public void OpenVictory()
    {
        CloseAll();
        OpenFrame(FrameType.Victory);
    }

    public void OpenDefeat()
    {
        CloseAll();
        OpenFrame(FrameType.Defeat);
    }

    public void OpenPreparing()
    {
        CloseAll();
        OpenFrame(FrameType.Preparing);
    }

    public void OpenWave()
    {
        CloseAll();
        OpenFrame(FrameType.Wave);
    }
}
