using System;
using TToTT.TowerDefense.UI.Button;

namespace TToTT.TowerDefense.UI
{
    public class MainMenuController : InterfaceContorller
    {
        private IUIButton _playButton;

        public void Init(Action onPlay, ButtonRegistry buttons)
        {
            _playButton = buttons.Get(ButtonId.Play);

            _playButton.OnClick += onPlay;
        }
    }
}