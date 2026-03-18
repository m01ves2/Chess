namespace Chess.Domain
{
    public class Move
    {
        public Position From { get; }
        public Position To { get; }
        public Piece Piece { get; }
        public Piece? CapturedPiece { get; }

        public Move(Position from, Position to, Piece piece, Piece? capturedPiece = null)
        {
            From = from;
            To = to;
            Piece = piece;
            CapturedPiece = capturedPiece;
        }
    }
}
