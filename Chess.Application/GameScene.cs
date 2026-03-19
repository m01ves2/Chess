using Chess.Domain;

namespace Chess.Application
{
    public class GameScene
    {
        public Board Board { get; set; } = new Board();
        public PieceColor CurrentPlayer { get; set; }
        public Player PlayerWhite { get; set; }
        public Player PlayerBlack { get; set; }
        public List<Move> MoveHistory { get; set; } = new List<Move>();

        public bool WhiteKingInCheck { get; set; } = false;
        public bool BlackKingInCheck { get; set; } = false;

        // Добавляем состояние сцены для UI
        // UI / игровое состояние
        public Position Cursor { get; set; } = new Position(0, 0); // позиция курсора
        public Position? SelectedPosition { get; set; } // выбранная клетка
        public List<Position> HighlightedPositions { get; } = new List<Position>(); // возможные ходы для фигуры на выбранной клетке

        public GameScene() 
        {

            PlayerWhite = new Player(PieceColor.White);
            PlayerBlack = new Player(PieceColor.Black);
            CurrentPlayer = PieceColor.White;
            Cursor = new Position(5, 4);
        }
    }
}
