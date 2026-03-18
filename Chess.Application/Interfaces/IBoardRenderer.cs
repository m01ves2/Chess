using Chess.Domain;

namespace Chess.Application.Interfaces
{
    public interface IBoardRenderer
    {
        void Render(GameScene game);
    }
}
