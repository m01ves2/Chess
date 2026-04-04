using Chess.Application.Views;
using Chess.Domain;
using Chess.Domain.Moves;

namespace Chess.Application.Players
{
    public class HumanPlayer : IPlayer
    {
        private readonly GameController _gameController;
        private Position? From;
        //private Position? To;
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
            // просто возвращает null, ход обрабатывается через Select и событие
            return null;
        }

        public void UndoMove()
        {
            Highlights.Clear();
            From = null;
        }

        public Move? Select(Position position, List<Move> moves)
        {
            // 1. если нет выбранной фигуры
            if (From == null) {
                var piece = _gameController.GamePosition.Board.GetSquare(position).Piece;

                if (piece != null && piece.Color == _gameController.GamePosition.CurrentPlayerColor) {
                    From = position;
                }

                return null;
            }

            // 2. если кликнули по своей фигуре — смена выбора
            var clickedPiece = _gameController.GamePosition.Board.GetSquare(position).Piece;

            if (clickedPiece != null &&
                clickedPiece.Color == _gameController.GamePosition.CurrentPlayerColor) {
                From = position;
                return null;
            }

            // 3. пытаемся найти ход
            var move = moves.FirstOrDefault(m => m.From == From && m.To == position);

            if (move == null)
                return null;

            // 4. promotion
            if (move is PromotionMove pm && pm.PromotionPiece == null) {
                _pendingPromotionMove = pm;
                return null;
            }

            // 5. успешный ход
            From = null;
            return move;
        }

        public Move? CompletePromotion(PieceViewType type)
        {
            if (_pendingPromotionMove == null)
                return null;
            var piece = Mapper.PieceViewTypeToPiece(type, _gameController.GamePosition.CurrentPlayerColor);
            //_pendingPromotionMove.SetPromotionPiece(piece);
            //return _pendingPromotionMove;
            var promotionMove = new PromotionMove(_pendingPromotionMove.From, _pendingPromotionMove.To, _pendingPromotionMove.Piece, _pendingPromotionMove.CapturedPiece);
            promotionMove.SetPromotionPiece(piece);
            _pendingPromotionMove = null;
            From = null;
            return promotionMove;
        }

        public void CancelPromotion()
        {
            _pendingPromotionMove = null;
            From = null;
        }

    }
}
