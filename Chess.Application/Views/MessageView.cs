namespace Chess.Application.Views
{
    public class CapturedView
    {
        public List<PieceViewType> WhiteCaptured { get; set; } = new List<PieceViewType>();
        public List<PieceViewType> BlackCaptured { get; set; } = new List<PieceViewType>();
    }
}
