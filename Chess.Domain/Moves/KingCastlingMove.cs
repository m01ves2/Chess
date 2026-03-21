namespace Chess.Domain.Moves
{
    public class KingCastlingMove : Move
    {
        public KingCastlingMove(Position from, Position to, Piece piece) : base(from, to, piece)
        {
        }

        public override void Apply(Board board, GameState state)
        {
            throw new NotImplementedException();
        }
    }
}
