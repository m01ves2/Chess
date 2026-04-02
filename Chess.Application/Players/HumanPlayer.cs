using Chess.Domain;
using Chess.Domain.Moves;
using Chess.Domain.Pieces;

namespace Chess.Application.Players
{
    public class HumanPlayer : IPlayer
    {
        private readonly GameController _gameController;
        private Position? From;
        private Position? To;
        private PromotionMove? _pendingPromotionMove;

        public HumanPlayer(GameController gameController)
        {
            _gameController = gameController;
        }

        //public Move ChooseMove(GamePosition position, IEnumerable<Move> moves)
        //{
        //    while (true) {
        //        var action = _input.ReadAction();

        //        _screenManager.HandleInput(action);

        //        //var move = _gameController.GetPendingMove();

        //        var move = _gameController.TryGetPendingMove();
        //        if (move != null && IsValid(move, moves)) {
        //            return move;
        //        }

        //        if (_screenManager.IsExitRequested)
        //            throw new ExitGameException();

        //    }
        //}

        //private bool IsValid(Move move, IEnumerable<Move> moves)
        //{
        //    return moves.Any(m => m.From == move.From && m.To == m.To);
        //}
        public Move? TryGetMove(List<Move> moves)
        {
            if (From == null || To == null) 
                return null;

            var move = moves.FirstOrDefault(m => m.From == From && m.To == To);

            if (move == null) 
                return null;

            if (move is PromotionMove pm) {
                //TODO - Дополнительные данные для Promotion
                pm.SetPromotionPiece(new Queen(_gameController.GamePosition.CurrentPlayer)); //TODO заглушка
            }

            From = null;
            To = null;
            return move;
        }

        public void Select(int row, int col)
        {
            Position position = new Position(row, col);
            //TODO Сброс подсветки highlights
            //TODO убрать подсветку с клетки From

            if (_gameController.GamePosition.CurrentPlayer == _gameController.GamePosition.Board.GetSquare(position).Piece?.Color) {
                From = position;
                //TODO установить подсветку на клетку From, установить highlights
                return;
            }
            
            if(From != null && From == position) {
                From = null;
                return;
            }


            if(From != null && To == null) {
                To = position; 
                return;
            }

            //// Сброс подсветки, если новая клетка выбрана
            //var selectedSquare = _gamePosition.Board.GetSquare(position);
            //_gameScene.HighlightedPositions.Clear();

            //if (_gameScene.SelectedPosition == position) {
            //    _gameScene.ClearSelection();
            //    return;
            //}

            //if (selectedSquare.Piece?.Color == _gamePosition.CurrentPlayer) {
            //    var result = SelectPiece(position);
            //    _gameScene.SetSelection(result.SelectedPosition!.Value);
            //    _gameScene.SetHighlights(result.AvailableMoves);
            //    return;
            //}

            //if (_gameScene.SelectedPosition != null) {
            //    TryMakeMove(_gameScene.SelectedPosition.Value, position);
            //    _gameScene.ClearSelection();
            //}
        }
    }
}
