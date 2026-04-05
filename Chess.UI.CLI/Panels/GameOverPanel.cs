using Chess.UI.CLI.Panels.BasePanels;
using Chess.UI.CLI.Panels.BasePanels.Rendering;
using Chess.UI.CLI.Views;

namespace Chess.UI.CLI.Panels
{
    public class GameOverPanel : TextPanelBase<GameOverView>
    {
        public GameOverPanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }

        public override void BuildBuffer(GameOverView view)
        {
            ClearBuffer();
            view.GameOverItems.ForEach(item => AddLine(new LineRender() { Text = item, Style = LineStyle.Selected}));
        }
    }
}
