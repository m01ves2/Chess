using Chess.Domain;

namespace Chess.Domain
{
    public class GameState
    {
        public bool WhiteKingMoved { get; set; } = false;
        public bool BlackKingMoved { get; set; } = false;

        public bool WhiteRookA_Moved { get; set; } = false;
        public bool WhiteRookH_Moved { get; set; } = false;

        public bool BlackRookA_Moved { get; set; } = false;
        public bool BlackRookH_Moved { get; set; } = false;

        public Position? EnPassantTarget { get; set; } = null;

        public GameState Clone()
        {
            return (GameState)this.MemberwiseClone(); // shallow copy достаточно, все поля value type
        }
    }
}
