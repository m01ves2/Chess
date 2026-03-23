using Chess.Domain.Pieces;

namespace Chess.Domain
{
    public class Board
    {
        public const int BoardSize = 8;
        public Square[,] Squares { get; } = new Square[8, 8];

        public List<Piece> BlackCaptured { get; } = new List<Piece>();
        public List<Piece> WhiteCaptured {  get; } = new List<Piece>();

        public Board(bool init = true)
        {
            for (int row = 0; row < BoardSize; row++)
                for (int col = 0; col < BoardSize; col++)
                    Squares[row, col] = new Square(new Position(row, col), null);

            if (init)
                InitStartingPosition();
        }

        public void InitStartingPosition()
        {
            // Пешки
            for (int col = 0; col < 8; col++) {
                Squares[1, col].Piece = new Pawn(PieceColor.Black);
                Squares[6, col].Piece = new Pawn(PieceColor.White);
            }

            // Ладьи
            Squares[0, 0].Piece = new Rook(PieceColor.Black);
            Squares[0, 7].Piece = new Rook(PieceColor.Black);
            Squares[7, 0].Piece = new Rook(PieceColor.White);
            Squares[7, 7].Piece = new Rook(PieceColor.White);

            // Кони
            Squares[0, 1].Piece = new Knight(PieceColor.Black);
            Squares[0, 6].Piece = new Knight(PieceColor.Black);
            Squares[7, 1].Piece = new Knight(PieceColor.White);
            Squares[7, 6].Piece = new Knight(PieceColor.White);

            // Слоны
            Squares[0, 2].Piece = new Bishop(PieceColor.Black);
            Squares[0, 5].Piece = new Bishop(PieceColor.Black);
            Squares[7, 2].Piece = new Bishop(PieceColor.White);
            Squares[7, 5].Piece = new Bishop(PieceColor.White);

            // Ферзи
            Squares[0, 3].Piece = new Queen(PieceColor.Black);
            Squares[7, 3].Piece = new Queen(PieceColor.White);

            // Короли
            Squares[0, 4].Piece = new King(PieceColor.Black);
            Squares[7, 4].Piece = new King(PieceColor.White);
        }

        public Square GetSquare(Position pos)
        {
            if (pos.Row < 0 || pos.Row > 7 || pos.Col < 0 || pos.Col > 7)
                throw new ArgumentOutOfRangeException($"Invalid board position: {pos.Row},{pos.Col}\nStack:\n{Environment.StackTrace}");
            return Squares[pos.Row, pos.Col];
        }
        public Position GetPosition(int row, int col) => Squares[row, col].Position;
        public bool IsInsideBoard(Position pos) => pos.Row >= 0 && pos.Row < 8 && pos.Col >= 0 && pos.Col < 8;

        public Board Clone()
        {
            var newBoard = new Board(false); // создаём пустую доску без инициализации
            for (int row = 0; row < BoardSize; row++) {
                for (int col = 0; col < BoardSize; col++) {
                    var piece = Squares[row, col].Piece;
                    newBoard.Squares[row, col] = new Square(new Position(row, col), piece?.Clone());
                }
            }

            newBoard.WhiteCaptured.AddRange(WhiteCaptured.Select(p => p.Clone()));
            newBoard.BlackCaptured.AddRange(BlackCaptured.Select(p => p.Clone()));

            return newBoard;
        }
    }
}
