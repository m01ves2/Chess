
namespace Chess.Domain.Pieces
{
    public class Bishop : Piece //офицер
    {
        public Bishop(PieceColor color) : base(color)
        {
        }

        public override Piece Clone()
        {
            return new Bishop(Color);
        }

        public override IEnumerable<MoveOffset> GetMoveOffsets()
        {
            return new MoveOffset[] {   new MoveOffset(1, 1, 7), 
                                        new MoveOffset(-1, -1, 7), 
                                        new MoveOffset(-1,1,7), 
                                        new MoveOffset(1,-1,7), 
            };
        }

        public override string ToString()
        {
            return base.ToString() + " bishop";
        }
    }
}
