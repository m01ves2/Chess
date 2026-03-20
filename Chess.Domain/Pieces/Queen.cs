
namespace Chess.Domain.Pieces
{
    public class Queen : Piece
    {
        public Queen(PieceColor color) : base(color)
        {
        }

        public override Piece Clone()
        {
            return new Queen(Color);
        }
        public override IEnumerable<MoveOffset> GetMoveOffsets()
        {
            return new MoveOffset[] {   new MoveOffset(0, 1, 7), 
                                        new MoveOffset(0, -1, 7),
                                        new MoveOffset(1, 0, 7), 
                                        new MoveOffset(-1, 0, 7),
                                        new MoveOffset(-1, 1, 7), 
                                        new MoveOffset(1, 1, 7),
                                        new MoveOffset(1, -1, 7), 
                                        new MoveOffset(-1, -1, 7)
            };
        }

        public override string ToString()
        {
            return base.ToString() + " queen";
        }
    }
}
