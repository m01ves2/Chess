using Chess.Domain;
using Chess.Domain.Pieces;
using System.Text;

namespace Chess.Application.Players.AI.Optimizers
{
    public static class TranspositionTable
    {
        public static string GetKey(GamePosition pos)
        {
            var sb = new StringBuilder();

            foreach (var square in pos.Board.Squares) {
                if (square.Piece == null)
                    sb.Append('.');
                else
                    sb.Append(PieceToChar(square.Piece));
            }

            sb.Append(pos.CurrentPlayerColor);

            return sb.ToString();
        }

        private static char PieceToChar(Piece piece) => piece switch
        {
            Rook r when r.Color == PieceColor.White => '\u2656',
            Rook r => '\u265C',
            Knight n when n.Color == PieceColor.White => '\u2658',
            Knight n => '\u265E',
            Bishop b when b.Color == PieceColor.White => '\u2657',
            Bishop b => '\u265D',
            Queen q when q.Color == PieceColor.White => '\u2655',
            Queen q => '\u265B',
            King k when k.Color == PieceColor.White => '\u2654',
            King k => '\u265A',
            Pawn p when p.Color == PieceColor.White => '\u2659',
            Pawn p => '\u265F',
            _ => '?'
        };
    }
}
