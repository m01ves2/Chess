using Chess.Application.ViewModels;
using Chess.UI.CLI.Panels.BasePanels;
using Chess.UI.CLI.Panels.BasePanels.Rendering;

namespace Chess.UI.CLI.Panels
{
    public class CapturedPanel : TextPanelBase<CapturedView>
    {
        public CapturedPanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }

        public override void BuildBuffer(CapturedView view)
        {
            ClearLines();

            AddLine(new LineRender() { Text = "Pieces captured:", Style = LineStyle.Selected });
            var whiteCapturedMessage = "";
            foreach (var capturedWhite in view.WhiteCaptured) {
                whiteCapturedMessage += GetSymbol(capturedWhite, PieceViewColor.White) + " ";
            }
            AddLine(new LineRender() { Text = whiteCapturedMessage, Style = LineStyle.None });

            var blackCapturedMessage = "";
            foreach (var capturedBlack in view.BlackCaptured) {
                blackCapturedMessage += GetSymbol(capturedBlack, PieceViewColor.Black) + " ";
            }
            AddLine(new LineRender() { Text = blackCapturedMessage, Style = LineStyle.None });
        }
    }
}
