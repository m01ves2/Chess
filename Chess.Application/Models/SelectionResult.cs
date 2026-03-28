using Chess.Domain;

namespace Chess.Application.Models
{
    public class SelectionResult
    {
        public Position? SelectedPosition { get; }
        public List<Position> AvailableMoves { get; }

        public SelectionResult(Position? selected, List<Position> moves)
        {
            SelectedPosition = selected;
            AvailableMoves = moves;
        }
    }
}
