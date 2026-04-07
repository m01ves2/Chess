using Chess.Domain;

namespace Chess.Application.Views
{
    public enum PieceViewType
    {
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }

    public enum PieceViewColor
    {
        White,
        Black,
        None
    }

    public class PieceView
    {
        public PieceViewType Type {  get; set; }
        public PieceViewColor Color { get; set; }
    }
    public class CellView
    {
        public PieceView? PieceView { get; set; }

        //public bool IsCursor { get; set; } = false;
        public bool IsSelected { get; set; } = false;
        public bool IsHighlighted { get; set; } = false;
    }

    public class BoardView
    {
        public CellView[,] Cells { get; set; }
        public PieceViewColor currentPlayer { get; set; } 
    }
}
