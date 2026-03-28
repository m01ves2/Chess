using Chess.UI.CLI.Models;

namespace Chess.UI.CLI.Interfaces
{
    public interface IInputHandler
    {
        PlayerAction ReadAction();
    }
}
