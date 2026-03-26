namespace TToTT.TowerDefense.UI
{
    public class UIFlowController // TODO: add state
    {
        private readonly UIWindowsController _windowsController;
        private readonly GameLoop _gameLoop;

        #region Init

        public UIFlowController(UIWindowsController windowsController, GameLoop gameLoop)
        {
            _windowsController = windowsController;
            _gameLoop = gameLoop;

            InitActions();
        }

        private void InitActions()
        {
            _gameLoop.OnGameDefeat += OpenDefeat;
            _gameLoop.OnGameVictory += OpenVictory;
            _gameLoop.OnWaveEnded += OpenPreparing;
            _gameLoop.OnWaveStarted += OpenWave;
        }

        #endregion

        #region Windows

        // Gameplay

        public void OpenGameplay()
        {
            _windowsController.OpenWindow(WindowType.Gameplay);
        }

        public void OpenPreparing()
        {
        }

        public void OpenDefeat()
        {
        }

        public void OpenVictory()
        {
        }

        public void OpenWave()
        {
        }

        //--------
        public void OpenMain()
        {
            _windowsController.OpenWindow(WindowType.Main);
        }

        #endregion
    }
}