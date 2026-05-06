using UnityEngine;

namespace TToTT.TowerDefense.UI
{
    public class UIFlowController
    {
        private readonly UIWindowsController _windows;
        private readonly GameStateMachine _gameState;
        private readonly MainMenuController _mainMenu;
        private readonly GameplayInterfaceController _gameplay;

        public UIFlowController(
            UIWindowsController windows,
            GameStateMachine gameState,
            MainMenuController mainMenu,
            GameplayInterfaceController gameplay)
        {
            _windows = windows;
            _gameState = gameState;
            _mainMenu = mainMenu;
            _gameplay = gameplay;

            _mainMenu.Init(OpenGameplay);
            _gameplay.Init(OpenMain);
            _windows.Init();

            _gameState.OnStateChanged += HandleGameStateChanged;

            OpenMain(); // начальный экран
        }

        private void HandleGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.WaveStarted:
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

#if UNITY_EDITOR
            Debug.Log($"Open {state}");
#endif

        }

        private void OpenMain()
        {

#if UNITY_EDITOR
            Debug.Log($"Open Main");
#endif

            _windows.OpenWindow(WindowType.Main);
            _windows.CloseWindow(WindowType.Gameplay);
        }

        private void OpenGameplay()
        {

#if UNITY_EDITOR
            Debug.Log($"Open Gameplay");
#endif

            _windows.CloseWindow(WindowType.Main);
            _windows.OpenWindow(WindowType.Gameplay);
            _gameplay.OpenPreparing();
        }
    }
}