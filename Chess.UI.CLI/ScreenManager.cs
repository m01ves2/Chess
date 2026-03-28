using Chess.Application.Models;
using Chess.UI.CLI.Models;
using Chess.UI.CLI.Screens;

namespace Chess.UI.CLI
{
    public class ScreenManager
    {
        private BaseScreen _currentScreen;
        public GameSettings GameSettings { get; private set; }

        public bool IsExitRequested { get; private set; } = false;

        public ScreenManager()
        {
            GameSettings = new GameSettings();
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
            if (action.Type == PlayerActionType.Escape) {
                //IsExitRequested = true;
                SetScreen(new MenuScreen(this));
                return;
            }

            _currentScreen?.HandleInput(action);
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
    }
}
