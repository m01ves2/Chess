using Chess.Domain;
using Chess.Domain.Moves;
using Chess.Domain.Pieces;
using Chess.Engine;

namespace Chess.Application.Players.AI.Optimizers
{
    public static class MoveRater
    {
        //функция вычисления предпочтительных ходов на основе оценки
        public static int ScoreMove(GamePosition position, Move move)
        {
            int score = 0;

            // 1. Capture (MVV-LVA)
            if (move.CapturedPiece != null) {
                score += GetPieceWeight(move.CapturedPiece) * 10
                       - GetPieceWeight(move.Piece);
            }

            // 2. Promotion
            if (move is PromotionMove)
                score += 100;

            // 3. Checks (да, через clone пока)
            if (GivesCheck(position, move))
                score += 50;

            // 4. Piece activity
            if (move.Piece is Knight or Bishop)
                score += 2;

            if (move.Piece is Queen)
                score -= 1;

            // 5. Pawn penalty
            if (move.Piece is Pawn && move.CapturedPiece == null)
                score -= 1;

            return score;
        }

        private static bool GivesCheck(GamePosition position, Move move)
        {
            var simPosition = position.Clone();
            var engine = new GameEngine();

            engine.MakeMove(simPosition, move);
            simPosition.SwitchTurn();

            return engine.IsKingInCheck(simPosition, simPosition.CurrentPlayerColor);
        }

        private static int GetPieceWeight(Piece piece) => piece switch
        {
            Pawn => 1,
            Knight => 3,
            Bishop => 3,
            Rook => 5,
            Queen => 9,
            King => 8,
            _ => 0
        };
    }
}
