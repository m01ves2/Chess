
namespace Chess.Domain.Pieces
{
    public class King : Piece
    {
        public King(PieceColor color) : base(color)
        {
        }

        public override Piece Clone()
        {
            return new King(Color);
        }

        public override IEnumerable<MoveOffset> GetMoveOffsets()
        {
            return new MoveOffset[] {   new MoveOffset(0, 1, 1),
                                        new MoveOffset(0, -1, 1), 
                                        new MoveOffset(1, 0, 1), 
                                        new MoveOffset(-1, 0, 1),
                                        new MoveOffset(1, 1, 1), 
                                        new MoveOffset(-1, -1, 1), 
                                        new MoveOffset(-1, 1, 1), 
                                        new MoveOffset(1, -1, 1)
            };
        }
    }
}
