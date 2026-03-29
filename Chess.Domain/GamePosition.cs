namespace Chess.Domain
{
    public class GamePosition
    {
        public Board Board { get; }
        public GameState State { get; }

        public PieceColor CurrentPlayer { get; private set; }

        public GamePosition(Board board, GameState state, PieceColor currentPlayer)
        {
            Board = board;
            State = state;
            CurrentPlayer = currentPlayer;
        }

        public void SwitchTurn()
        {
            CurrentPlayer = CurrentPlayer == PieceColor.White
                ? PieceColor.Black
                : PieceColor.White;
        }
        public GamePosition Clone() => new GamePosition(Board.Clone(), State.Clone(), CurrentPlayer);

        public IEnumerable<Square> GetSquaresWithPlayerPieces(PieceColor player)
        {
            List<Square> squares = new List<Square>();
            foreach (var square in Board.Squares) {
                if(!square.IsEmpty() && square.Piece.Color == player)
                    squares.Add(square);
            }
            return squares;
        }

    }
}
