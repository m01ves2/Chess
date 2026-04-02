using Chess.Domain;

namespace Chess.Application.Views
{
    public static class NotationMapper
    {
        public static Position Parse(string notation)
        {
            if (notation.Length > 2)
                throw new ArgumentException("Invalid notation of square");

            char file = notation[0];
            int col = file - 'a';
            if (col < 0 || col > 7)
                throw new ArgumentException("Invalid column notation of square");

            char rank = notation[1];
            int row = Board.BoardSize - (rank - '0');
            if (row < 0 || row > 7)
                throw new ArgumentException("Invalid row notation of square");

            return new Position(row, col);
        }

        public static (char file, char rank) PositionToChars(Position position)
        {
            char rank = (char)('0' + (Board.BoardSize - position.Row));
            char file = (char)('a' + position.Col);
            return (file, rank);
        }

        public static string PositionToString(Position position)
        {
            (char file, char rank) = PositionToChars(position);
            return $"{file}{rank}";
        }
    }
}
