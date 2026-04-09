using Chess.Domain;

namespace Chess.Application.Models
{
    public class GameInfo
    {
        public bool WhiteKingInCheck { get; set; } = false;
        public bool BlackKingInCheck { get; set; } = false;
    }
}
