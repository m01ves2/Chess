namespace Chess.Domain.Moves
{
    public class PawnDoubleMove : Move
    {
        public Position EnPassentPosition { get; }
        public PawnDoubleMove(Position from, Position to, Piece piece, Position enPassentPosition) : base(from, to, piece)
        {
            EnPassentPosition = enPassentPosition;
        }

        public override void Apply(GamePosition gamePosition)
        {
            var board = gamePosition.Board;
            var state = gamePosition.State;
            board.Squares[To.Row, To.Col].Piece = Piece;
            board.Squares[From.Row, From.Col].Piece = null;
            state.EnPassantTarget = EnPassentPosition;
        }
    }
}
