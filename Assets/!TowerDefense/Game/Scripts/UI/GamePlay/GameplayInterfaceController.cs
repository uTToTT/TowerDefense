using System;
using TToTT.TowerDefense.UI.Button;

namespace TToTT.TowerDefense.UI
{
    public class GameplayInterfaceController : InterfaceContorller
    {
        private IUIButton _fromVictoryToMain;
        private IUIButton _fromDefeatToMain;
        private IUIButton _startWave;

        public void Init(Action onBackToMain,Action startWave, ButtonRegistry buttons)
        {
            _fromVictoryToMain = buttons.Get(ButtonId.FromVictoryToMain);
            _fromDefeatToMain = buttons.Get(ButtonId.FromDefeatToMain);
            _startWave = buttons.Get(ButtonId.StartWave);

            _fromVictoryToMain.OnClick += onBackToMain;
            _fromDefeatToMain.OnClick += onBackToMain;
            _startWave.OnClick += startWave;
        }

        public void OpenVictory() { CloseAll(); OpenFrame(FrameType.Victory); }
        public void OpenDefeat() { CloseAll(); OpenFrame(FrameType.Defeat); }
        public void OpenPreparing() { CloseAll(); OpenFrame(FrameType.Preparing); }
        public void OpenWave() { CloseAll(); OpenFrame(FrameType.Wave); }
    }
}