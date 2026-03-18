
namespace Chess.Domain.Pieces
{
    public class Pawn : Piece //пешка
    {
        public Pawn(PieceColor color) : base(color)
        {
        }

        public override Piece Clone()
        {
            return new Pawn(Color);
        }
        public override IEnumerable<MoveOffset> GetMoveOffsets()
        {   
            var dir = this.Color == PieceColor.White ? -1 : 1;
            return new MoveOffset[] { new MoveOffset( 0, dir, 1),
                                      new MoveOffset( 0, dir, 2, MoveType.PawnFirstMove),
                                      new MoveOffset( 1, dir, 1, MoveType.PawnAttack),
                                      new MoveOffset(-1, dir, 1, MoveType.PawnAttack),
            };
        }
    }
}
