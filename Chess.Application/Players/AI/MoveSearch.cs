using Chess.Application.Views;
using Chess.Domain;
using Chess.Engine;
using System.Diagnostics;
using System.Drawing;

namespace Chess.Application.Players.AI
{
    public class MoveSearch
    {
        private readonly GameController _gameController;
        private readonly int _depth;
        private readonly PieceColor _aiColor;
        private readonly GameEngine _engine = new GameEngine();
        private static readonly Random _random = new Random();

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
            moves = moves.OrderByDescending(move => MoveRater.ScoreMove(move)).ToList();
            foreach (var move in moves) {
                var simPosition = _gameController.GamePosition.Clone(); // клон всей позиции
                var simEngine = new GameEngine();
                simEngine.MakeMove(simPosition, move);
                simPosition.SwitchTurn(); // переключаем ход

                int score = Minimax(simPosition, _depth, int.MinValue, int.MaxValue);

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
            return bestMoves[_random.Next(bestMoves.Count)];
        }

        private int Minimax(GamePosition position, int depth, int alpha, int beta)
        {
            if (depth == 0)
                return Evaluator.Evaluate(position, _aiColor);

            var currentColor = position.CurrentPlayerColor;
            var moves = _engine.GetAllLegalMoves(position, currentColor);
            if (!moves.Any()) {
                if (_engine.IsKingInCheck(position, currentColor))
                    return currentColor == _aiColor ? -10000 : +10000;
                else
                    return 0; // пат
            }

            bool isMaximizing = currentColor == _aiColor;
            int bestEval = isMaximizing ? int.MinValue : int.MaxValue;

            foreach (var move in moves) {
                var simPosition = position.Clone();       // клон всей позиции
                var simEngine = new GameEngine();
                simEngine.MakeMove(simPosition, move);
                simPosition.SwitchTurn();                 // переключаем ход

                int eval = Minimax(simPosition, depth - 1, alpha, beta);

                if (isMaximizing) {
                    bestEval = Math.Max(bestEval, eval);
                    alpha = Math.Max(alpha, bestEval);
                }
                else {
                    bestEval = Math.Min(bestEval, eval);
                    beta = Math.Min(beta, bestEval);
                }

                // Alpha-Beta pruning
                if (beta <= alpha)
                    break;
            }

            return bestEval;
        }
    }
}
