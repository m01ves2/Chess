using Chess.Application;
using Chess.Application.Interfaces;
using Chess.Domain;
using System.Security.Cryptography.X509Certificates;

namespace Chess.UI.CLI
{
    public class CLIInputHandler : IInputHandler
    {
        public PlayerAction ReadAction()
        {
            var keyPressed = Console.ReadKey().Key;
            switch (keyPressed) {
                case ConsoleKey.Spacebar:
                    return new PlayerAction(PlayerActionType.Select);
                case ConsoleKey.LeftArrow:
                    return new PlayerAction(PlayerActionType.MoveLeft);
                case ConsoleKey.RightArrow:
                    return new PlayerAction(PlayerActionType.MoveRight);
                case ConsoleKey.UpArrow:
                    return new PlayerAction(PlayerActionType.MoveUp);
                case ConsoleKey.DownArrow:
                    return new PlayerAction(PlayerActionType.MoveDown);
                case ConsoleKey.Q:
                    return new PlayerAction(PlayerActionType.Quit);
                case ConsoleKey.N:
                    return new PlayerAction(PlayerActionType.NewGame);
                case ConsoleKey.U:
                    return new PlayerAction(PlayerActionType.Undo);
                default:
                    return new PlayerAction(PlayerActionType.None);
            }
        }
    }
}
