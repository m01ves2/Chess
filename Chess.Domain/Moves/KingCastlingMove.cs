using Chess.Domain.Pieces;
using System.Drawing;

namespace Chess.Domain.Moves
{
    public class KingCastlingMove : Move
    {
        public bool IsKingSide { get; }
        public KingCastlingMove(Position from, Position to, Piece piece, bool isKingSide) : base(from, to, piece)
        {
            IsKingSide = isKingSide;
        }

        public override void Apply(GamePosition gamePosition)
        {
            var board = gamePosition.Board;
            var state = gamePosition.State;
            if (IsKingSide) { //короткая рокировка
                ApplyShortCastling(board, state);
            }
            else {
                ApplyLongCastling(board, state);
            }
        }

        //private void ApplyCastlingMove(Board board, GameState state)
        //{
        //    board.Squares[To.Row, To.Col].Piece = Piece;
        //    board.Squares[From.Row, From.Col].Piece = null;

        //    //var delta = new MoveOffset(1, 0, 1);
        //    //var rookOldPosition = To + delta;
        //    //var rook = board.GetSquare(rookOldPosition).Piece;
        //    //var rookNewPosition = To - delta;
        //    //board.Squares[rookNewPosition.Row, rookNewPosition.Col].Piece = rook;
        //    //board.Squares[rookOldPosition.Row, rookOldPosition.Col].Piece = null;

        //    Position rookPosition;
        //    Position rookNewPosition;
        //    Piece rook;
        //    if (Piece.Color == PieceColor.White) {
        //        rookPosition = new Position(7, 7);
        //        rookNewPosition = new Position(7, 5);
        //    }
        //    else {
        //        rookPosition = new Position(0, 7);
        //        rookNewPosition = new Position(0, 5);
        //    }
        //    rook = board.GetSquare(rookPosition).Piece!;
        //    board.Squares[rookNewPosition.Row, rookNewPosition.Col].Piece = rook;
        //    board.Squares[rookPosition.Row, rookPosition.Col].Piece = null;

        //    if (Piece.Color == PieceColor.White) { //TODO!!!!!
        //        state.WhiteKingMoved = true;
        //        state.WhiteRookH_Moved = true;
        //    }
        //    else {
        //        state.BlackKingMoved = true;
        //        state.BlackRookH_Moved = true;
        //    }
        //}

        //private void ApplyLongCastlingMove(Board board, GameState state)
        //{
        //    //board.Squares[To.Row, To.Col].Piece = Piece;
        //    //board.Squares[From.Row, From.Col].Piece = null;

        //    //var delta = new MoveOffset(1, 0, 1);
        //    //var rookOldPosition = To - delta - delta;
        //    //var rook = board.GetSquare(rookOldPosition).Piece;
        //    //var rookNewPosition = To + delta;
        //    //board.Squares[rookNewPosition.Row, rookNewPosition.Col].Piece = rook;
        //    //board.Squares[rookOldPosition.Row, rookOldPosition.Col].Piece = null;

        //    Position rookPosition;
        //    Position rookNewPosition;
        //    Piece rook;
        //    if (Piece.Color == PieceColor.White) {
        //        rookPosition = new Position(7, 0);
        //        rookNewPosition = new Position(7, 3);
        //    }
        //    else {
        //        rookPosition = new Position(0, 0);
        //        rookNewPosition = new Position(0, 3);
        //    }
        //    rook = board.GetSquare(rookPosition).Piece!;
        //    board.Squares[rookNewPosition.Row, rookNewPosition.Col].Piece = rook;
        //    board.Squares[rookPosition.Row, rookPosition.Col].Piece = null;

        //    if (Piece.Color == PieceColor.White) { //TODO!!!!!
        //        state.WhiteKingMoved = true;
        //        state.WhiteRookA_Moved = true;
        //    }
        //    else {
        //        state.BlackKingMoved = true;
        //        state.BlackRookA_Moved = true;
        //    }
        //}

        private void ApplyShortCastling(Board board, GameState state)
        {
            // Move the king
            board.Squares[To.Row, To.Col].Piece = Piece;
            board.Squares[From.Row, From.Col].Piece = null;

            // Move the rook - short castling
            int rookRow = From.Row;
            int rookFromCol = 7; // h1/h8
            int rookToCol = 5;   // f1/f8

            var rook = board.Squares[rookRow, rookFromCol].Piece!;
            board.Squares[rookRow, rookToCol].Piece = rook;
            board.Squares[rookRow, rookFromCol].Piece = null;

            // Update state
            if (Piece.Color == PieceColor.White) {
                state.WhiteKingMoved = true;
                state.WhiteRookH_Moved = true;
            }
            else {
                state.BlackKingMoved = true;
                state.BlackRookH_Moved = true;
            }
        }

        private void ApplyLongCastling(Board board, GameState state)
        {
            board.Squares[To.Row, To.Col].Piece = Piece;
            board.Squares[From.Row, From.Col].Piece = null;

            int rookRow = From.Row;
            int rookFromCol = 0; // a1/a8
            int rookToCol = 3;   // d1/d8

            var rook = board.Squares[rookRow, rookFromCol].Piece!;
            board.Squares[rookRow, rookToCol].Piece = rook;
            board.Squares[rookRow, rookFromCol].Piece = null;

            if (Piece.Color == PieceColor.White) {
                state.WhiteKingMoved = true;
                state.WhiteRookA_Moved = true;
            }
            else {
                state.BlackKingMoved = true;
                state.BlackRookA_Moved = true;
            }
        }
    }
}
