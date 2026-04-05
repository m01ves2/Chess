using Chess.UI.CLI.Panels.BasePanels;
using Chess.UI.CLI.Panels.BasePanels.Rendering;
using Chess.UI.CLI.Views;

namespace Chess.UI.CLI.Panels
{
    public class QuitPanel : TextPanelBase<QuitView>
    {
        public QuitPanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }

        public override void BuildBuffer(QuitView view)
        {
            ClearBuffer();
            //TODO show game statistics
            view.QuitItems.ForEach(item => AddLine(new LineRender() { Text = item, Style = LineStyle.None })); 
        }
    }
}
