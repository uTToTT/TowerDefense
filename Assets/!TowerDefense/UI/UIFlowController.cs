using TToTT.TowerDefense.UI.Button;

namespace TToTT.TowerDefense.UI
{
    public class UIFlowController
    {
        private readonly UIWindowsController _windows;
        private readonly GameStateMachine _gameState;
        private readonly MainMenuController _mainMenu;
        private readonly GameplayInterfaceController _gameplay;
        private readonly ButtonRegistry _buttons;

        public UIFlowController(
            UIWindowsController windows,
            GameStateMachine gameState,
            MainMenuController mainMenu,
            GameplayInterfaceController gameplay,
            ButtonRegistry buttons)
        {
            _windows = windows;
            _gameState = gameState;
            _mainMenu = mainMenu;
            _gameplay = gameplay;
            _buttons = buttons;

            _mainMenu.Init(OpenGameplay, buttons);
            _gameplay.Init(OpenMain, buttons);
            _windows.Init();

            _gameState.OnStateChanged += HandleGameStateChanged;

            OpenMain();
        }

        private void HandleGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.LevelStarted:
                    _gameplay.OpenWave();
                    break;
                case GameState.WaveEnded:
                    _gameplay.OpenPreparing();
                    break;
                case GameState.GameVictory:
                    _gameplay.OpenVictory();
                    break;
                case GameState.GameDefeat:
                    _gameplay.OpenDefeat();
                    break;
            }
        }

        private void OpenMain()
        {
            _windows.OpenWindow(WindowType.Main);
            _windows.CloseWindow(WindowType.Gameplay);
        }

        private void OpenGameplay()
        {
            _windows.CloseWindow(WindowType.Main);
            _windows.OpenWindow(WindowType.Gameplay);
            _gameplay.OpenPreparing();
        }
    }
}