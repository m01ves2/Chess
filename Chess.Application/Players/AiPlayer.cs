using Chess.Domain;
using Chess.Domain.Moves;
using Chess.Domain.Pieces;

namespace Chess.Application.Players
{
    public class AiPlayer : IPlayer
    {
        private readonly Random _random = new Random();
        private int _aiDifficulty = 1;
        private GameController _gameController;

        public AiPlayer(GameController gameController, int aiDifficulty) {
            _gameController = gameController;
            _aiDifficulty = aiDifficulty;
        }
        public Move? TryGetMove(List<Move> moves)
        {
            var move = moves[_random.Next(moves.Count)];
            if (move is PromotionMove pm) {
                pm.SetPromotionPiece(new Queen(_gameController.GamePosition.CurrentPlayerColor));
            }
            return move;
        }
    }
}
