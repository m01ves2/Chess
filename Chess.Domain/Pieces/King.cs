
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
            var dir = this.Color == PieceColor.White ? -1 : 1;
            return new MoveOffset[] {   new MoveOffset(0, 1, 1),
                                        new MoveOffset(0, -1, 1), 
                                        new MoveOffset(1, 0, 1), 
                                        new MoveOffset(-1, 0, 1),
                                        new MoveOffset(1, 1, 1), 
                                        new MoveOffset(-1, -1, 1), 
                                        new MoveOffset(-1, 1, 1), 
                                        new MoveOffset(1, -1, 1),
                                        //TODO add KingCastling
                                        new MoveOffset(3, 0, 1, MoveType.KingCastling ),
                                        new MoveOffset(-4, 0, 1, MoveType.KingLongCastling ),
            };
        }
    }
}
