using Chess.Application.Players.AI;
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
        private MoveSearch _minimax;
        private PieceColor _aiColor;

        public AiPlayer(GameController gameController, int aiDifficulty, PieceColor pieceColor ) {
            _gameController = gameController;
            _aiDifficulty = aiDifficulty;
            _aiColor = pieceColor;
            _minimax = new MoveSearch(_gameController, aiDifficulty, _aiColor);
        }

        public Move? TryGetMove()
        {
            return _minimax.FindBestMove();
        }

        //public Move? TryGetMove(List<Move> moves)
        //{
        //    var move = moves[_random.Next(moves.Count)];
        //    if (move is PromotionMove pm) {
        //        pm.SetPromotionPiece(new Queen(_gameController.GamePosition.CurrentPlayerColor));
        //    }
        //    return move;
        //}
    }
}
