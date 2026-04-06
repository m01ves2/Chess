using Chess.Application.Models;
using Chess.UI.CLI.Panels.BasePanels;
using Chess.UI.CLI.Panels.BasePanels.Rendering;
using Chess.UI.CLI.Views;

namespace Chess.UI.CLI.Panels
{
    public class SettingsPanel : TextPanelBase<SettingsView>
    {
        public SettingsPanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }

        public override void BuildBuffer(SettingsView view)
        {
            ClearBuffer();

            for (int i = 0; i < view.Items.Count; i++) {
                var menuItem = view.Items[i];
                AddLine(new LineRender() { Text = menuItem, Style = (i == view.SelectedIndex ? LineStyle.Selected : LineStyle.None) });
            }
        }
    }
}
