using Chess.Domain;

namespace Chess.Engine
{
    public class GameState
    {
        public bool WhiteKingMoved { get; set; }
        public bool BlackKingMoved { get; set; }

        public bool WhiteRookA_Moved { get; set; }
        public bool WhiteRookH_Moved { get; set; }

        public bool BlackRookA_Moved { get; set; }
        public bool BlackRookH_Moved { get; set; }

        public Position? EnPassantTarget { get; set; }

        public GameState Clone()
        {
            return (GameState)this.MemberwiseClone(); // shallow copy достаточно, все поля value type
        }
    }
}
