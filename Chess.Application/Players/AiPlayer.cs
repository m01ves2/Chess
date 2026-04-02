using Chess.Application;
using Chess.Domain;

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
            return moves[_random.Next(moves.Count)];
        }
    }
}
