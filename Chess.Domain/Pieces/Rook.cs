
namespace Chess.Domain.Pieces
{
    public class Rook : Piece
    {
        public Rook(PieceColor color) : base(color) //ладья
        {
        }

        public override Piece Clone()
        {
            return new Rook(Color);
        }
        public override IEnumerable<MoveOffset> GetMoveOffsets()
        {
            return new MoveOffset[] {   new MoveOffset(1, 0, 7),
                                        new MoveOffset(-1, 0, 7),
                                        new MoveOffset(0, 1, 7),
                                        new MoveOffset(0, -1, 7),
            };
        }
    }
}
