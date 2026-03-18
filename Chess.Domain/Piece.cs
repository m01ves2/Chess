namespace Chess.Domain
{
    public enum PieceColor
    {
        White,
        Black
    }
    public abstract class Piece
    {
        public PieceColor Color { get; }
        protected Piece(PieceColor color)
        {
            Color = color;
        }

        // Функция возвращает потенциальные ходы
        public abstract IEnumerable<MoveOffset> GetMoveOffsets();
        public abstract Piece Clone();
    }

}
