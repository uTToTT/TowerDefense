using UnityEngine;

public class GameplayInterfaceController : MonoBehaviour
{
    [SerializeField] private ButtonWrapper _buttonRestartGame;
    [SerializeField] private ButtonWrapper _buttonToMain;

    public void Init()
    {
        _buttonRestartGame.OnClick += GameManager.Instance.RestartGame;
        _buttonToMain.OnClick += () =>
        {
            UIManager.Instance.CloseWindow(WindowType.Defeat);
            UIManager.Instance.CloseWindow(WindowType.Gameplay);
            UIManager.Instance.OpenWindow(WindowType.Main);
        };
    }

    public void EnableWaveFrames()
    {

    }

    public void EnablePreparingFrames()
    {

    }
}
