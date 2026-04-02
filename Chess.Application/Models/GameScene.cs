using Chess.Domain;

namespace Chess.Application.Models
{
    public class GameScene
    {
        //public Board Board { get; set; }
        //public PieceColor CurrentPlayer { get; set; }
        //public Player PlayerWhite { get; set; }
        //public Player PlayerBlack { get; set; }

        public bool WhiteKingInCheck { get; set; } = false;
        public bool BlackKingInCheck { get; set; } = false;


        // Добавляем состояние сцены для UI
        // UI / игровое состояние
        //public Position Cursor { get; set; } = new Position(0, 0); // позиция курсора
        //public Position? SelectedPosition { get; private set; } // выбранная клетка
        //public List<Position> HighlightedPositions { get; } = new List<Position>(); // возможные ходы для фигуры на выбранной клетке
        
        //private List<Move> _moveHistory = new();
        //public IReadOnlyList<Move> MoveHistory => _moveHistory;


        public GameScene() 
        {

            //PlayerWhite = new Player(PieceColor.White);
            //PlayerBlack = new Player(PieceColor.Black);
            //CurrentPlayer = PieceColor.White;
            //Cursor = new Position(5, 4);
        }

        //public void UpdateMoveHistory(IEnumerable<Move> moves)
        //{
        //    _moveHistory.Clear();
        //    _moveHistory.AddRange(moves);
        //}

        //private bool HasSelection()
        //{
        //    return SelectedPosition != null;
        //}

        //public void SetSelection(Position pos)
        //{
        //    SelectedPosition = pos;
        //}
        //public void ClearSelection()
        //{
        //    SelectedPosition = null;
        //}
        //public void SetHighlights(IEnumerable<Position> positions)
        //{
        //    HighlightedPositions.Clear();
        //    HighlightedPositions.AddRange(positions);
        //}
        //public void ClearHighlights()
        //{
        //    HighlightedPositions.Clear();
        //}
    }
}
