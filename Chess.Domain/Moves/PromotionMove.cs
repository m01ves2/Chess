namespace Chess.Domain.Moves
{
    public class PromotionMove : Move
    {
        public Piece PromotionPiece { get; }

        public bool IsChoicePending { get; set; } = true; // пока игрок не выбрал фигуру
        public Type? PromotedPieceType { get; set; } // Queen, Rook, Bishop, Knight     // результат выбора игрока

        public PromotionMove(Position from, Position to, Piece piece, bool IsChoicePending) : base(from, to, piece)
        {
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
