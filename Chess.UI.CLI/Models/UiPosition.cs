namespace Chess.UI.CLI.Models
{
    public class UiPosition
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public UiPosition(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public override string ToString() => $"{(char)('a' + Col)}{Row + 1}";

        public override bool Equals(object? obj) => obj is UiPosition other && this == other;
        public override int GetHashCode() => HashCode.Combine(Row, Col);

        public static bool operator ==(UiPosition a, UiPosition b) => a.Row == b.Row && a.Col == b.Col;
        public static bool operator !=(UiPosition a, UiPosition b) => !(a == b);
    }
}
