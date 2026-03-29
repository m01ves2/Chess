namespace Chess.Domain.Moves
{
    public class PromotionMove : Move
    {
        public Piece? PromotionPiece { get; private set; }

        //public bool IsChoicePending { get; set; } = true; // пока игрок не выбрал фигуру
        //public Type? PromotedPieceType { get; set; } // Queen, Rook, Bishop, Knight     // результат выбора игрока

        public PromotionMove(Position from, Position to, Piece piece) : base(from, to, piece)
        {
        }

        public override void Apply(GamePosition gamePosition)
        {
            if (PromotionPiece == null) return;
            var board = gamePosition.Board;
            board.Squares[To.Row, To.Col].Piece = PromotionPiece;
            board.Squares[From.Row, From.Col].Piece = null;
        }

        public void SetPromotionPiece(Piece promotionPiece)
        {
            PromotionPiece = promotionPiece;
        }
    }
}
