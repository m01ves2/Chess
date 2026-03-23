using Chess.Domain;

namespace Chess.Application.Interfaces
{
    public interface IPlayer
    {
        Move ChooseMove(GamePosition position, IEnumerable<Move> moves);
    }
}
