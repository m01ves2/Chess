using Chess.UI.CLI.Interfaces;
using Chess.UI.CLI.Models;

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
                case ConsoleKey.Escape:
                    return new PlayerAction(PlayerActionType.Escape);
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
