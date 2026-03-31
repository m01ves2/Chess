using Chess.Application;
using Chess.Application.Models;
using Chess.UI.CLI.Models;
using Chess.UI.CLI.Screens;
using Chess.UI.CLI.Screens.BaseScreens;

namespace Chess.UI.CLI
{
    public class ScreenManager
    {
        private BaseScreen _currentScreen;
        public GameSettings GameSettings { get; private set; }
        public bool IsExitRequested { get; private set; } = false;
        public bool IsStartGameRequested { get; private set; } = false;

        private GameController _gameController;

        public ScreenManager(GameController gameController)
        {
            GameSettings = new GameSettings();
            _gameController = gameController;
        }

        public void SetScreen(BaseScreen screen)
        {
            _currentScreen = screen;
        }

        public void Render()
        {
            if (Console.WindowWidth < 80 || Console.WindowHeight < 40) {
                RenderResizeWarning();
                return;
            }

            _currentScreen?.Render();
        }

        public void HandleInput(PlayerAction action)
        {
            var isHandled = _currentScreen.HandleInput(action); //event spreading emulation

            if (action.Type == PlayerActionType.Escape && !isHandled) {
                //IsExitRequested = true;
                SetScreen(new MenuScreen(this, _gameController));
                return;
            }

        }

        private void RenderResizeWarning()
        {
            Console.Clear();
            Console.SetCursorPosition(2, 2);
            Console.WriteLine("Console too small!\n Minimum 80x40");
        }

        public void RequestExit()
        {
            IsExitRequested = true;
        }

        public void RequestStartGame()
        {
            IsStartGameRequested = true;
        }
    }
}
