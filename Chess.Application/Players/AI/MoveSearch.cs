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

        public MoveSearch(GameController gameController, int aiDifficulty, PieceColor aiColor)
        {
            _gameController = gameController;
            _depth = aiDifficulty;
            _aiColor = aiColor;
        }

        //public Move? FindBestMove()
        //{
        //    var moves = _gameController.GetAllLegalMoves();
        //    int bestScore = int.MinValue;
        //     List<Move> bestMoves = new List<Move>();

        //    //System.Diagnostics.Debug.WriteLine($"AI sees {moves.Count()} moves");
        //    foreach (var move in moves) {
        //        //   System.Diagnostics.Debug.WriteLine($"{move.ToString()}");

        //        _gameController.DoMove(move, false);
        //        int score = Minimax(_depth);
        //        _gameController.UndoMove(false);



        //        if (score > bestScore) {
        //            bestScore = score;
        //            bestMoves.Clear();
        //            bestMoves.Add(move);
        //        }
        //        else if (score == bestScore) {
        //            bestMoves.Add(move);
        //        }
        //    }

        //    if (bestMoves.Count == 0)
        //        return null;

        //    // Выбираем случайный ход среди лучших
        //    var random = new Random();
        //    return bestMoves[random.Next(bestMoves.Count)];
        //}

        public Move? FindBestMove()
        {
            var moves = _gameController.GetAllLegalMoves();
            int bestScore = int.MinValue;
            List<Move> bestMoves = new List<Move>();

            foreach (var move in moves) {
                var simPosition = _gameController.GamePosition.Clone(); // клон всей позиции
                var simEngine = new GameEngine();
                simEngine.MakeMove(simPosition, move);
                simPosition.SwitchTurn(); // переключаем ход

                int score = Minimax(simPosition, _depth - 1, simPosition.CurrentPlayerColor);

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

        private int Minimax(GamePosition position, int depth, PieceColor currentColor)
        {
            if (depth == 0)
                return Evaluator.Evaluate(position, _aiColor);

            var _engine = new GameEngine();

            var moves = _engine.GetAllLegalMoves(position, currentColor).ToList();
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

                int eval = Minimax(simPosition, depth - 1, simPosition.CurrentPlayerColor);

                if (isMaximizing)
                    bestEval = Math.Max(bestEval, eval);
                else
                    bestEval = Math.Min(bestEval, eval);
            }

            return bestEval;
        }

        //private int Minimax(int depth)
        //{
        //    if (depth == 0)
        //        return Evaluator.Evaluate(_gameController.GamePosition, _aiColor);

        //    var moves = _gameController.GetAllLegalMoves();
        //    if (!moves.Any()) {
        //        var currentColor = _gameController.GamePosition.CurrentPlayerColor;
        //        if (_gameController.IsKingInCheck(_gameController.GamePosition, currentColor))
        //            return currentColor == _aiColor ? -10000 : +10000;
        //        else
        //            return 0; // пат
        //    }

        //    bool isMaximizing = _gameController.GamePosition.CurrentPlayerColor == _aiColor; //isMaximizing=true - это наш ход, иначе - соперника.
        //    int bestEval = isMaximizing ? int.MinValue : int.MaxValue;  

        //    foreach (var move in moves) {

        //        _gameController.DoMove(move, false);
        //        int eval = Minimax(depth - 1);
        //        _gameController.UndoMove(false);

        //        if (isMaximizing)
        //            bestEval = Math.Max(bestEval, eval); //наш ход
        //        else
        //            bestEval = Math.Min(bestEval, eval); //ход соперника
        //    }

        //    return bestEval;
        //}
    }
}
