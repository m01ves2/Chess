namespace Chess.Domain
{
    public class Square
    {
        // Если используем Position:
        public Position Position { get; }

        public Piece? Piece { get; set; }

        public Square(Position position, Piece? piece = null)
        {
            Piece = piece;
            Position = position;
        }

        public bool IsEmpty() => Piece == null;
    }
}
