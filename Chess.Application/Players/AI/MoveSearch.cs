using Chess.Application.Players.AI.Optimizers;
using Chess.Domain;
using Chess.Domain.Moves;
using Chess.Domain.Pieces;
using Chess.Engine;

namespace Chess.Application.Players.AI
{
    public class MoveSearch
    {
        private readonly GameController _gameController;
        private readonly int _depth;
        private readonly PieceColor _aiColor;
        private readonly GameEngine _engine = new GameEngine();
        private static readonly Random _random = new Random();
        Dictionary<string, int> _cache = new();

        int _ttHits = 0;
        int _ttMisses = 0;
        int totalMovesProcessed = 0;

        public MoveSearch(GameController gameController, int aiDifficulty, PieceColor aiColor)
        {
            _gameController = gameController;
            _depth = aiDifficulty;
            _aiColor = aiColor;
        }

        public Move? FindBestMove()
        {
            _ttHits = 0;
            _ttMisses = 0;

            var moves = _gameController.GetAllLegalMoves();
            int bestScore = int.MinValue;
            List<Move> bestMoves = new List<Move>();
            moves = moves.OrderByDescending(move => MoveRater.ScoreMove(_gameController.GamePosition, move)).ToList();
            totalMovesProcessed += moves.Count();
            foreach (var move in moves) {

                if(move is PromotionMove) {
                    PromotionMoveCasting(move);
                }

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

            System.Diagnostics.Debug.WriteLine($"_ttHits: {_ttHits}, _ttMisses: {_ttMisses}, 100%*_ttHits / (_ttHits + _ttMisses): {100 * (float)_ttHits / (_ttHits + _ttMisses)} ");
            System.Diagnostics.Debug.WriteLine($"totalMovesProcessed: {totalMovesProcessed}");
            totalMovesProcessed = 0;

            // Выбираем случайный ход среди лучших
            return bestMoves[_random.Next(bestMoves.Count)];
        }

        private int Minimax(GamePosition position, int depth, int alpha, int beta)
        {
            if (depth == 0)
                return Evaluator.Evaluate(position, _aiColor);

            var key = TranspositionTable.GetKey(position);
            if (_cache.TryGetValue(key, out var cached)) {
                _ttHits++;
                return cached;
            }
            else {
                _ttMisses++;
            }

            var currentColor = position.CurrentPlayerColor;
            var moves = _engine.GetAllPseudoMoves(position, currentColor).OrderByDescending(m => MoveRater.ScoreMove(position, m)).ToList();
            totalMovesProcessed += moves.Count();

            if (!moves.Any()) {
                if (_engine.IsKingInCheck(position, currentColor))
                    return currentColor == _aiColor ? -10000 : +10000;
                else
                    return 0; // пат
            }

            bool isMaximizing = currentColor == _aiColor;
            int bestEval = isMaximizing ? int.MinValue : int.MaxValue;
            bool cutoff = false;

            foreach (var move in moves) {
                if (move is PromotionMove) {
                    PromotionMoveCasting(move);
                }

                var simPosition = position.Clone();       // клон всей позиции
                var simEngine = new GameEngine();
                simEngine.MakeMove(simPosition, move);
                if (_engine.IsKingInCheck(simPosition, currentColor))
                    continue;

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
                if (beta <= alpha) {
                    cutoff = true;
                    break;
                }
            }

            //if (!cutoff)
                _cache[key] = bestEval;
            return bestEval;
        }

        private void PromotionMoveCasting(Move move)
        {
            //if (move is PromotionMove pm && pm.PromotionPiece == null) {
            //    move = new PromotionMove(pm.From, pm.To, pm.Piece, pm.CapturedPiece);
            //    ((PromotionMove)move).SetPromotionPiece(new Queen(pm.Piece.Color));
            //}

            if (move is PromotionMove pm && pm.PromotionPiece == null) {
                pm.SetPromotionPiece(new Queen(pm.Piece.Color));
            }
        }
    }
}
