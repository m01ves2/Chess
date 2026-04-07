using Chess.Application.Views;
using Chess.Domain;

namespace Chess.Application.Players.AI
{
    public class MoveSearch
    {
        private readonly GameController _gameController;
        private readonly int _depth;
        private readonly PieceColor _aiColor;

        public MoveSearch(GameController gameController, int aiDifficulty, PieceColor aiColor)
        {
            _gameController = gameController;
            _depth = aiDifficulty;
            _aiColor = aiColor;
        }

        public Move? FindBestMove()
        {
            var moves = _gameController.GetAllLegalMoves();
            int bestScore = int.MinValue;
            List<Move> bestMoves = new List<Move>();

            foreach (var move in moves) {
                _gameController.DoMove(move, false);
                int score = Minimax(_depth);
                _gameController.UndoMove(false);

                if (score > bestScore) {
                    bestScore = score;
                    bestMoves.Clear();
                    bestMoves.Add(move);
                }
                else if (score == bestScore) {
                    bestMoves.Add(move);
                }
            }

            if (bestMoves.Count == 0)
                return null;

            // Выбираем случайный ход среди лучших
            var random = new Random();
            return bestMoves[random.Next(bestMoves.Count)];
        }

        private int Minimax(int depth)
        {
            if (depth == 0)
                return Evaluator.Evaluate(_gameController.GamePosition, _aiColor);

            var moves = _gameController.GetAllLegalMoves();
            if (!moves.Any()) {
                var currentColor = _gameController.GamePosition.CurrentPlayerColor;
                if (_gameController.IsKingInCheck(_gameController.GamePosition, currentColor))
                    return currentColor == _aiColor ? -10000 : +10000;
                else
                    return 0; // пат
            }

            bool isMaximizing = _gameController.GamePosition.CurrentPlayerColor == _aiColor; //isMaximizing=true - это наш ход, иначе - соперника.
            int bestEval = isMaximizing ? int.MinValue : int.MaxValue;  

            foreach (var move in moves) {
                _gameController.DoMove(move, false);
                int eval = Minimax(depth - 1);
                _gameController.UndoMove(false);

                if (isMaximizing)
                    bestEval = Math.Max(bestEval, eval); //наш ход
                else
                    bestEval = Math.Min(bestEval, eval); //ход соперника
            }

            return bestEval;
        }
    }
}
