namespace Chess.Domain
{

    public abstract class Move
    {
        public Position From { get; }
        public Position To { get; }
        public Piece Piece { get; }
        public Move(Position from, Position to, Piece piece)
        {
            From = from;
            To = to;
            Piece = piece;
        }

        public abstract void Apply(GamePosition gamePosition);
        
    }


}
