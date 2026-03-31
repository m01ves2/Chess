using Chess.Domain;

namespace Chess.CompositionRoot.Players
{
    public interface IPlayer
    {
        Move ChooseMove(GamePosition position, IEnumerable<Move> moves);
    }
}
