namespace Chess.Domain.Moves
{
    public class NormalMove : Move
    {
        public Piece? CapturedPiece { get; }

        public NormalMove(Position from, Position to, Piece piece, Piece? capturedPiece = null) : base(from, to, piece)
        {
            CapturedPiece = capturedPiece;
        }

        public override void Apply(Board board, GameState state)
        {
            board.Squares[To.Row, To.Col].Piece = Piece;
            board.Squares[From.Row, From.Col].Piece = null;

            if (CapturedPiece != null) {
                if (CapturedPiece.Color == PieceColor.White)
                    board.WhiteCaptured.Add(CapturedPiece);
                else
                    board.BlackCaptured.Add(CapturedPiece);
            }
        }
    }
}
