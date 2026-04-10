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
                score += GetPieceWeight(move.CapturedPiece) * 10 - GetPieceWeight(move.Piece);
            }

            // 2. Promotion
            if (move is PromotionMove)
                score += 100;

            if (move is KingCastlingMove)
                score += 50;

            // 3. Checks (да, через clone пока)
            if (GivesCheck(position, move))
                score += 50;

            // 4. Piece activity
            if (move.Piece is Knight or Bishop)
                score += 10;

            if (move.Piece is Queen)
                score -= 5;

            // 5. Pawn penalty
            if (move.Piece is Pawn && move.CapturedPiece == null)
                score -= 1;

            // 6. Move piece to the center
            if (IsCenter4x4(move.To)) { //если фигуры Ai толпятся в центре - они больше контролируют пространства. поощряем
                score += 1;
            }

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
            Pawn => 2,
            Knight => 5,
            Bishop => 5,
            Rook => 10,
            Queen => 50,
            King => 40,
            _ => 0
        };

        private static bool IsCenter4x4(Position pos)
        {
            return pos.Row >= 2 && pos.Row <= 5 &&
                   pos.Col >= 2 && pos.Col <= 5;
        }

        private static int ScoresForPromotionalMove(Move move)
        {
            var score = 0;
            var piece = move.Piece;

            if (piece is Knight && !((move.From.Row == 0 && (move.From.Col == 1 || move.From.Col == 6) && piece.Color == PieceColor.Black) ||
                                     (move.From.Row == 7 && (move.From.Col == 1 || move.From.Col == 6) && piece.Color == PieceColor.White)))
                score += 1;

            if (piece is Bishop && !((move.From.Row == 0 && (move.From.Col == 2 || move.From.Col == 5) && piece.Color == PieceColor.Black) ||
                                     (move.From.Row == 7 && (move.From.Col == 2 || move.From.Col == 5) && piece.Color == PieceColor.White)))
                score += 1;

            if (piece is Rook   && !((move.From.Row == 0 && (move.From.Col == 0 || move.From.Col == 7) && piece.Color == PieceColor.Black) ||
                                     (move.From.Row == 7 && (move.From.Col == 0 || move.From.Col == 7) && piece.Color == PieceColor.White)))
                score += 1;

            if (piece is Queen &&  !((move.From.Row == 0 && move.From.Col == 3 && piece.Color == PieceColor.Black) ||
                                     (move.From.Row == 7 && move.From.Col == 3 && piece.Color == PieceColor.White)))
                score += 1;

            if (piece is Pawn && !((move.From.Row == 1 && piece.Color == PieceColor.Black) ||
                                     (move.From.Row == 6 && piece.Color == PieceColor.White)))
                score += 1;

            return score;
        }
    }
}
