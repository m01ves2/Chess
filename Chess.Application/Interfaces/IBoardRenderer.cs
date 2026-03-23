using Chess.Domain;

namespace Chess.Application.Interfaces
{
    public interface IBoardRenderer
    {
        void Render(GamePosition gamePosition, GameScene gameScene);
    }
}
