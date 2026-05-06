using System;
using UnityEngine;

namespace TToTT.TowerDefense.UI
{
    public class GameplayInterfaceController : InterfaceContorller
    {
        [SerializeField] private ButtonWrapper _victoryButtonToMain;
        [SerializeField] private ButtonWrapper _defeatButtonToMain;

        public void Init(Action onBackToMain)
        {
            _victoryButtonToMain.OnClick += onBackToMain;
            _defeatButtonToMain.OnClick += onBackToMain;
        }

        public void OpenVictory() { CloseAll(); OpenFrame(FrameType.Victory); }
        public void OpenDefeat() { CloseAll(); OpenFrame(FrameType.Defeat); }
        public void OpenPreparing() { CloseAll(); OpenFrame(FrameType.Preparing); }
        public void OpenWave() { CloseAll(); OpenFrame(FrameType.Wave); }
    }
}