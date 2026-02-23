using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private ButtonWrapper _playButton;

    public void Init()
    {
        _playButton.OnClick += () =>
        {
            UIManager.Instance.CloseWindow(WindowType.Main);
            UIManager.Instance.OpenWindow(WindowType.Gameplay);
        };
    }
}
