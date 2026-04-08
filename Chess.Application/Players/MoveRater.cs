using Chess.Domain;
using Chess.Domain.Moves;
using Chess.Domain.Pieces;

namespace Chess.Application.Players
{
    public static class MoveRater
    {
        public static int ScoreMove(Move move)
        {
            var score = 0;
            if (move is NormalMove || move is EnPassantMove) {
                var attacker = move.Piece;
                var captured = move.CapturedPiece;
                if(captured != null)
                    score += GetPieceWeight(captured) * 10 - GetPieceWeight(attacker);
            }
            else if(move is PromotionMove ) {
                score += 7;
            }
            return score;
        }

        private static int GetPieceWeight(Piece piece) => piece switch
        {
            Pawn => 1,
            Knight => 3,
            Bishop => 3,
            Rook => 5,
            Queen => 9,
            King => 9,
            _ => 0
        };
    }
}
