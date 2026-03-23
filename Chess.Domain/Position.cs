using System.Diagnostics;

namespace Chess.Domain
{
    public struct Position
    {
        public int Row { get; }  // 0..7
        public int Col { get; }  // 0..7

        public Position(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public override string ToString() => $"{(char)('a' + Col)}{Row + 1}";

        public override bool Equals(object? obj) => obj is Position other && this == other;
        public override int GetHashCode() => HashCode.Combine(Row, Col);

        public static bool operator== (Position a, Position b) => a.Row == b.Row && a.Col == b.Col;
        public static bool operator!= (Position a, Position b) => !(a == b);
    
        public static Position operator+ (Position pos, MoveOffset moveOffset)
        {
            return new Position(pos.Row + moveOffset.Dy, pos.Col + moveOffset.Dx);
        }

        public static Position operator+ (MoveOffset moveOffset, Position pos)
        {
            return new Position(pos.Row + moveOffset.Dy, pos.Col + moveOffset.Dx);
        }

        public static Position operator- (Position pos, MoveOffset moveOffset)
        {
            return new Position(pos.Row - moveOffset.Dy, pos.Col - moveOffset.Dx);
        }
    }
}
