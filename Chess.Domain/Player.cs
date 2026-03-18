namespace Chess.Domain
{
    public class Player
    {
        public string Name { get; set; }
        public PieceColor Color { get; set; }
        public bool IsAI { get; set; } = false;

        public Player(PieceColor color, string name = "Player1", bool isAI = false) 
        { 
            Name = name;
            Color = color;
            IsAI = isAI;
        }
    }
}
