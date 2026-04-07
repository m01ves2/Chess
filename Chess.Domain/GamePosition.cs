namespace Chess.Domain
{
    public class GamePosition
    {
        public Board Board { get; }
        public GameState State { get; }

        public PieceColor CurrentPlayerColor { get; private set; }

        public GamePosition(Board board, GameState state, PieceColor currentPlayer)
        {
            Board = board;
            State = state;
            CurrentPlayerColor = currentPlayer;
        }

        public void SwitchTurn()
        {
            CurrentPlayerColor = CurrentPlayerColor == PieceColor.White
                ? PieceColor.Black
                : PieceColor.White;
        }
        public GamePosition Clone() => new GamePosition(Board.Clone(), State.Clone(), CurrentPlayerColor);

        public IEnumerable<Square> GetSquaresWithPlayerPieces(PieceColor player)
        {
            List<Square> squares = new List<Square>();
            foreach (var square in Board.Squares) {
                if (!square.IsEmpty() && square.Piece!.Color == player)
                    squares.Add(square);
            }
            return squares;
        }

        public IEnumerable<Piece> GetAllPieces()
        {
            List<Piece> pieces = new List<Piece>();
            foreach (var square in Board.Squares) {
                if (!square.IsEmpty())
                    pieces.Add(square.Piece!);
            }
            return pieces;
        }
    }
}
