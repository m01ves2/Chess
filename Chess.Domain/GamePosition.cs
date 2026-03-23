namespace Chess.Domain
{
    public class GamePosition
    {
        public Board Board { get; }
        public GameState State { get; }

        public PieceColor CurrentPlayer { get; private set; }

        public List<Move> MoveHistory { get; set; } = new List<Move>();

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

    }
}
