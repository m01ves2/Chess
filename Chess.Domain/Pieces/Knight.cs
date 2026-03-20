
namespace Chess.Domain.Pieces
{
    public class Knight : Piece //конь
    {
        public Knight(PieceColor color) : base(color)
        {
        }

        public override Piece Clone()
        {
            return new Knight(Color);
        }
        public override IEnumerable<MoveOffset> GetMoveOffsets()
        {
            return new MoveOffset[] {   new MoveOffset(2, 1, 1), 
                                        new MoveOffset(2, -1, 1), 
                                        new MoveOffset(-2, 1, 1), 
                                        new MoveOffset(-2, -1, 1),
                                        new MoveOffset(1, 2, 1), 
                                        new MoveOffset(1, -2, 1), 
                                        new MoveOffset(-1, 2, 1), 
                                        new MoveOffset(-1, -2, 1) 
            };
        }

        public override string ToString()
        {
            return base.ToString() + " knight";
        }
    }
}