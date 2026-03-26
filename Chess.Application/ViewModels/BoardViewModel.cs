using Chess.Domain;

namespace Chess.Application.ViewModels
{
    public class BoardViewModel
    {
        public char[,] Cells = new char[Board.BoardSize,Board.BoardSize];

        public Position Cursor {  get; set; }
        public List<Position> HighlightedPositions { get; } = new List<Position>();
    }
}
