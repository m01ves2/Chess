using Chess.Application;
using Chess.Application.ViewModels;
using Chess.Domain;
using Chess.UI.CLI.Panels.BasePanels;
using Chess.UI.CLI.Panels.BasePanels.Rendering;

namespace Chess.UI.CLI.Panels
{
    public class HistoryPanel : TextPanelBase<MoveHistoryView>
    {
        private int skip;
        private int take;
        private bool canScrollDown =  false;
        public HistoryPanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
            skip = 0;
            take = InnerHeight - 2;
        }

        public override void BuildBuffer(MoveHistoryView view)
        {
            ClearLines();
            var scrolledHistory = view.Moves.Skip(skip).Take(take).ToList();
            canScrollDown = scrolledHistory.Count > 0 && view.Moves.Count > InnerHeight - 2;

            if (skip > 0 && view.Moves.Count > take )
                AddLine(new LineRender() { Text = "↑ [PageUp]" });

            for (int i = 0; i < scrolledHistory.Count; i++) {
                AddLine(new LineRender() { Text = scrolledHistory[i], Style = LineStyle.None } );
            }

            if (view.Moves.Count > skip + take)
                AddLine(new LineRender() { Text = "↓ [PageDown]" });
        }

        public void PageUp()
        {
            if (skip > 0) {
                skip--;
            }
        }

        public void PageDown()
        {
            if(canScrollDown)
                skip++;
        }
    }
}
