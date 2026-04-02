using Chess.Application.Views;
using Chess.UI.CLI.Panels.BasePanels;
using Chess.UI.CLI.Panels.BasePanels.Rendering;

namespace Chess.UI.CLI.Panels
{
    public class InfoPanel : TextPanelBase<InfoView>
    {
        public InfoPanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }

        public override void BuildBuffer(InfoView view)
        {
            ClearLines();

            AddLine(new LineRender() { Text = $"{view.CurrentPlayer} turn", Style = LineStyle.None });

            if (view.IsCheck)
                AddLine(new LineRender() { Text = $"King in Check!", Style = LineStyle.Selected });

            if (view.IsPromoted)
                AddLine(new LineRender() { Text = "Your pawn is being promoted! Choose piece", Style = LineStyle.Selected });
        }
    }
}
