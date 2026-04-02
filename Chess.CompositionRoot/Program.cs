using Chess.Application;
using Chess.UI.CLI;
using Chess.UI.CLI.Interfaces;
using Chess.UI.CLI.Screens;

namespace Chess.CompositionRoot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if(!InitializeConsole()) {
                Console.WriteLine("Can't start chess game");
            }

            GameController gameController = new GameController();
            IInputHandler inputHandler = new CLIInputHandler();
            ScreenManager screenManager = new ScreenManager(gameController, inputHandler);
            screenManager.SetScreen(new MenuScreen(screenManager, gameController));
            
            //GameLoop gameLoop = new GameLoop(gameController, screenManager, inputHandler);
            //gameLoop.Run();
            screenManager.Run();
        }


        static bool InitializeConsole()
        {
            try {
                Console.WindowHeight = 1;
                Console.WindowWidth = 1;
                Console.SetBufferSize(80, 40);
                Console.SetWindowSize(80, 40);
                Console.OutputEncoding = System.Text.Encoding.UTF8;
            }
            catch {
                // ignore (например, Windows Terminal может не дать)
                // если не получилось — пробуем хотя бы так
                Console.WindowHeight = 40;
                Console.WindowWidth = 80;
            }

            Console.CursorVisible = false;

            if (Console.WindowWidth < 80 || Console.WindowHeight < 40) {
                Console.Clear();
                Console.WriteLine("Please resize console to at least 80x40");
                Console.ReadKey();
                return false;
            }

            return true;
        }
    }
}