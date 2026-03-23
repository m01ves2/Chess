using Chess.Domain;

namespace Chess.Application
{
    public class GameSnapshot
    {
        public Board Board { get; }
        public GameState State { get; }
        public PieceColor CurrentPlayer { get; }

        public GameSnapshot(Board board, GameState state, PieceColor currentPlayer)
        {
            Board = board;
            State = state;
            CurrentPlayer = currentPlayer;
        }

        public GamePosition ToGamePosition()
        {
            return new GamePosition(Board, State, CurrentPlayer);
        }
    }
}
