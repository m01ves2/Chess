using Chess.Domain;
using Chess.Domain.Pieces;

namespace Chess.Application.Players.AI
{
    public static class Evaluator
    {
        public static int Evaluate(GamePosition position, PieceColor playerColor)
        {
            int score = 0;
            var allPieces = position.GetAllPieces();
            foreach (var piece in allPieces) {
                int value = piece switch
                {
                    Pawn => 1,
                    Knight => 3,
                    Bishop => 3,
                    Rook => 5,
                    Queen => 9,
                    King => 0,
                    _ => 0
                };
                score += piece.Color == playerColor ? value : -value;
            }
            return score;
        }
    }
}
