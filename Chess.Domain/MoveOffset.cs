namespace Chess.Domain
{
    public class MoveOffset
    {
        public int Dx { get; }
        public int Dy { get; }
        public int MaxDistance { get; } // 1 для прыжков, 7 для скользящих, 2 для пешки на первый ход

        public MoveOffset(int dx, int dy, int maxDistance)
        {
            Dx = dx;
            Dy = dy;
            MaxDistance = maxDistance;
        }

        public static MoveOffset operator* (MoveOffset mo, int step)
        {
            return new MoveOffset(mo.Dx * step, mo.Dy * step, mo.MaxDistance);
        }

        public static MoveOffset operator *(int step, MoveOffset mo)
        {
            return new MoveOffset(mo.Dx * step, mo.Dy * step, mo.MaxDistance);
        }
    }
}

//алгоритм построения клеток по направлениям
//foreach offset in piece.Offsets
//    for step = 1..offset.MaxDistance
//        target = currentPos + offset * step
//        if target вне доски → break
//        square = board.GetSquare(target)
//        if square.Piece == null
//            legalMoves.Add(target)
//        else if square.Piece.Color != piece.Color
//            legalMoves.Add(target)
//            break
//        else // союзная фигура
//    break