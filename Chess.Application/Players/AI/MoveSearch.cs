using Chess.Application.Players.AI.Optimizers;
using Chess.Domain;
using Chess.Domain.Moves;
using Chess.Domain.Pieces;
using Chess.Engine;
using System.IO;

namespace Chess.Application.Players.AI
{
    public class MoveSearch
    {
        private readonly GameController _gameController;
        private readonly int _depth;
        private readonly PieceColor _aiColor;
        private readonly GameEngine _engine = new GameEngine();
        private static readonly Random _random = new Random();
        private Dictionary<string, int> _cache = new(); //кеш для рассчета позиций, которые уже встречались

        int _ttHits = 0;
        int _ttMisses = 0;
        int _totalMovesProcessed = 0;

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
            _totalMovesProcessed += moves.Count();
            foreach (var move in moves) {

                if(move is PromotionMove) {
                    PromotionMoveCasting(move);
                }

                var simPosition = _gameController.GamePosition.Clone(); // клон всей позиции
                var simEngine = new GameEngine();
                simEngine.MakeMove(simPosition, move);
                simPosition.SwitchTurn(); // переключаем ход

                HashSet<string> path = new HashSet<string>(); //путь для выявления циклов в принятии решений Ai, чтобы их избегать.

                int score = Minimax(simPosition, _depth, int.MinValue, int.MaxValue, path);

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

            //System.Diagnostics.Debug.WriteLine($"_ttHits: {_ttHits}, _ttMisses: {_ttMisses}, 100%*_ttHits / (_ttHits + _ttMisses): {100 * (float)_ttHits / (_ttHits + _ttMisses)} ");
            //System.Diagnostics.Debug.WriteLine($"totalMovesProcessed: {totalMovesProcessed}");
            //totalMovesProcessed = 0;

            // Выбираем случайный ход среди лучших
            return bestMoves[_random.Next(bestMoves.Count)];
        }

        private int Minimax(GamePosition position, int depth, int alpha, int beta, HashSet<string> path)
        {
            if (depth == 0)
                return Evaluator.Evaluate(position, _aiColor);
                //return Quiescence(position, alpha, beta);

            var key = TranspositionTable.GetKey(position);
            if (_cache.TryGetValue(key, out var cached)) {
                _ttHits++;
                return cached;
            }
            else {
                _ttMisses++;
            }

            if (path.Contains(key)) { //отслеживание позиций, которые уже встречались
                return 0; // ничья / повтор
            }
            path.Add(key); //зафикцировали то, что уже встречали позицию

            var currentColor = position.CurrentPlayerColor;
            var moves = _engine.GetAllPseudoMoves(position, currentColor).OrderByDescending(m => MoveRater.ScoreMove(position, m)).ToList();
            _totalMovesProcessed += moves.Count();

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
                _engine.MakeMove(simPosition, move);
                if (_engine.IsKingInCheck(simPosition, currentColor))
                    continue;

                simPosition.SwitchTurn();                 // переключаем ход

                int eval = Minimax(simPosition, depth - 1, alpha, beta, path);

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
            path.Remove(key); //удаляем из сохранённых позиций

            if (!cutoff)
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

        private int Quiescence(GamePosition position, int alpha, int beta)
        {
            int standPat = Evaluator.Evaluate(position, _aiColor);

            if (standPat >= beta)
                return beta;

            if (standPat > alpha)
                alpha = standPat;

            var currentColor = position.CurrentPlayerColor;

            // только ВЗЯТИЯ
            var moves = _engine.GetAllPseudoMoves(position, currentColor).Where(m => m.CapturedPiece != null);

            foreach (var move in moves) {
                if (move is PromotionMove) {
                    PromotionMoveCasting(move);
                }

                var simPosition = position.Clone();
                _engine.MakeMove(simPosition, move);

                if (_engine.IsKingInCheck(simPosition, currentColor))
                    continue;

                simPosition.SwitchTurn();

                int score = -Quiescence(simPosition, -beta, -alpha); // negamax стиль

                if (score >= beta)
                    return beta;

                if (score > alpha)
                    alpha = score;
            }

            return alpha;
        }
    }
}
