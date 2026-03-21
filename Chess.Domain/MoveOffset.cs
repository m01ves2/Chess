namespace Chess.Domain
{
    public enum MoveType
    {
        Normal,
        PawnAttack,
        PawnFirstMove,
        PawnEnPassant,
        KingCastling,
        KingLongCastling,
    }

    public class MoveOffset
    {
        public int Dx { get; }
        public int Dy { get; }
        public int MaxDistance { get; } // 1 для прыжков, 7 для скользящих, 2 для пешки на первый ход
        public MoveType MoveType { get; }

        public MoveOffset(int dx, int dy, int maxDistance, MoveType type = MoveType.Normal)
        {
            Dx = dx;
            Dy = dy;
            MaxDistance = maxDistance;
            MoveType = type;
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