namespace Chess.Domain.Moves
{
    public class EnPassantMove : Move
    {
        public Position CapturedPiecePosition { get; }
        public EnPassantMove(Position from, Position to, Piece piece, Position capturedPiecePosition, Piece capturedPiece) : base(from, to, piece, capturedPiece)
        {
            CapturedPiecePosition = capturedPiecePosition;
        }

        public override void Apply(GamePosition gamePosition)
        {
            var board = gamePosition.Board;
            board.Squares[To.Row, To.Col].Piece = Piece;
            board.Squares[From.Row, From.Col].Piece = null;
            
            if (CapturedPiece.Color == PieceColor.White)
                board.WhiteCaptured.Add(CapturedPiece);
            else
                board.BlackCaptured.Add(CapturedPiece);

            board.Squares[CapturedPiecePosition.Row, CapturedPiecePosition.Col].Piece = null;
        }

    }
}
