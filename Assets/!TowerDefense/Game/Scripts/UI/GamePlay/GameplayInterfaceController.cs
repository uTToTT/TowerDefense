using System;
using TToTT.TowerDefense.UI.Button;

namespace TToTT.TowerDefense.UI
{
    public class GameplayInterfaceController : InterfaceContorller
    {
        private IUIButton _fromVictoryToMainButton;
        private IUIButton _fromDefeatToMainButton;

        public void Init(Action onBackToMain, ButtonRegistry buttons)
        {
            _fromVictoryToMainButton = buttons.Get(ButtonId.FromVictoryToMain);
            _fromDefeatToMainButton = buttons.Get(ButtonId.FromDefeatToMain);

            _fromVictoryToMainButton.OnClick += onBackToMain;
            _fromDefeatToMainButton.OnClick += onBackToMain;
        }

        public void OpenVictory() { CloseAll(); OpenFrame(FrameType.Victory); }
        public void OpenDefeat() { CloseAll(); OpenFrame(FrameType.Defeat); }
        public void OpenPreparing() { CloseAll(); OpenFrame(FrameType.Preparing); }
        public void OpenWave() { CloseAll(); OpenFrame(FrameType.Wave); }
    }
}