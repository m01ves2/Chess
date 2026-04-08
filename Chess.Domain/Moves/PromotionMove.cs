namespace Chess.Domain.Moves
{
    public class PromotionMove : Move
    {
        public Piece? PromotionPiece { get; private set; }
        public PromotionMove(Position from, Position to, Piece piece, Piece? capturedPiece = null) : base(from, to, piece, capturedPiece)
        {
        }

        public override void Apply(GamePosition gamePosition)
        {
            if (PromotionPiece == null) return;
            var board = gamePosition.Board;
            board.Squares[To.Row, To.Col].Piece = PromotionPiece;
            board.Squares[From.Row, From.Col].Piece = null;

            if (CapturedPiece != null) {
                if (CapturedPiece.Color == PieceColor.White)
                    board.WhiteCaptured.Add(CapturedPiece);
                else
                    board.BlackCaptured.Add(CapturedPiece);
            }
        }

        public void SetPromotionPiece(Piece promotionPiece)
        {
            PromotionPiece = promotionPiece;
        }
    }
}
