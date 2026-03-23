namespace Chess.Domain.Moves
{
    internal class PromotionMove : Move
    {
        public Piece PromotionPiece { get; }
        public PromotionMove(Position from, Position to, Piece piece, Piece promotionPiece) : base(from, to, piece)
        {
            PromotionPiece = promotionPiece;
        }

        public override void Apply(GamePosition gamePosition)
        {
            //if (Piece.Color == PieceColor.White && To.Row == 0)
            //    piece = new Queen(PieceColor.White);
            //else if (pawn.Color == PieceColor.Black && pos.Row == 7)
            //    piece = new Queen(PieceColor.Black);
        }
    }
}
