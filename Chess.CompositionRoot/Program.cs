using Chess.Application;
using Chess.Application.Interfaces;
using Chess.Domain;
using Chess.Engine;
using Chess.UI.CLI;

namespace Chess.CompositionRoot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GameScene game = new GameScene();
            IBoardRenderer renderer = new CLIBoardRenderer();
            IInputHandler inputHandler = new CLIInputHandler();

            GameController gameController = new GameController(game);
            GameLoop gameLoop = new GameLoop(gameController, renderer, inputHandler);
            gameLoop.Run();
        }
    }
}