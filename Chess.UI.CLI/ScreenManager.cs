using Chess.Application;
using Chess.Application.Models;
using Chess.UI.CLI.Interfaces;
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
        //public bool IsStartGameRequested { get; private set; } = false;

        private GameController _gameController;
        private readonly IInputHandler _inputHandler;

        public ScreenManager(GameController gameController, IInputHandler inputHandler)
        {
            GameSettings = new GameSettings();
            _gameController = gameController;
            _inputHandler = inputHandler;
        }

        public void Run()
        {
            var frameTime = TimeSpan.FromMilliseconds(16);

            while (!IsExitRequested) {  // ЕДИНСТВЕННЫЙ цикл

            var start = DateTime.Now;
                
                Render();                                   // рисует всё: меню, панели, доску
                var action = _inputHandler.ReadAction();    // читает пользовательский ввод. если играет Ai vs Ai...
                if (action != null)
                    HandleInput(action);                        // передаёт ввод текущему экрану
                
                _currentScreen.Tick();                      // двигает логику экрана (игру или ничего)

                var elapsed = DateTime.Now - start;
                var sleep = frameTime - elapsed;

                if (sleep > TimeSpan.Zero)
                    Thread.Sleep(sleep);
            }
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
            //if (action.Type == PlayerActionType.None) 
            //    return;

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

        //public void RequestStartGame()
        //{
        //    IsStartGameRequested = true;
        //}
    }
}
