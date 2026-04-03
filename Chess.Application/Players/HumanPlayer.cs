using Chess.Application.Views;
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
        public bool IsPromotionPending => _pendingPromotionMove != null;

        public List<Position> Highlights { get; private set; } = new List<Position>();
        //private List<Move> _moves;
        public Position? SelectedFrom => From;

        public HumanPlayer(GameController gameController)
        {
            _gameController = gameController;
        }

        public Move? TryGetMove(List<Move> moves)
        {
            if (From == null || To == null)
                return null;


            Move? move;

            if (_pendingPromotionMove != null) {
                move = _pendingPromotionMove;
                _pendingPromotionMove = null;
            }
            else {
                move = moves.FirstOrDefault(m => m.From == From && m.To == To);
                if (move == null)
                    return null;
            }

            if (move is PromotionMove pm && pm.PromotionPiece == null) { //Дополнительные данные для Promotion
                _pendingPromotionMove = pm;
                return null;
            }

            From = null;
            To = null;
            return move;
        }

        public void UndoMove()
        {
            Highlights.Clear();
            From = null;
            To = null;
        }
        public void CompletePromotion(PieceViewType type)
        {
            if (_pendingPromotionMove == null)
                return;
            var piece = Mapper.PieceViewTypeToPiece(type, _gameController.GamePosition.CurrentPlayerColor);
            _pendingPromotionMove.SetPromotionPiece(piece);
        }

        public void CancelPromotion()
        {
            _pendingPromotionMove = null;
            From = null;
        }

        public void Select(int row, int col)
        {
            Position position = new Position(row, col);
            //TODO Сброс подсветки highlights
            Highlights.Clear();

            //TODO убрать подсветку с клетки From
            if (From != null && From == position) {
                From = null;
                return;
            }

            if (_gameController.GamePosition.CurrentPlayerColor == _gameController.GamePosition.Board.GetSquare(position).Piece?.Color) {
                From = position;
                //TODO установить подсветку на клетку From, установить highlights
                return;
            }

            if (From != null && To == null) {
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
