using Chess.Domain;

namespace Chess.Application.Players
{
    public interface IPlayer
    {
        Move? TryGetMove();
    }
}
