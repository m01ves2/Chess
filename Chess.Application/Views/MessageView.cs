namespace Chess.Application.ViewModels
{
    public class CapturedView
    {
        public List<PieceViewType> WhiteCaptured { get; set; } = new List<PieceViewType>();
        public List<PieceViewType> BlackCaptured { get; set; } = new List<PieceViewType>();
    }
}
