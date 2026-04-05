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

            //for (int i = 0; i < view.SettingsItems.Count; i++) {
            //    var menuItem = view.SettingsItems[i];
            //    AddLine(new LineRender() { Text = menuItem, Style = (i == view.selectedIndex ? LineStyle.Selected : LineStyle.None) });
            //}
            string playerWhite = view.PlayerTypeWhite == PlayerType.Human ? "Human" : "Ai";
            string playerBlack = view.PlayerTypeBlack == PlayerType.Human ? "Human" : "Ai";
            AddLine(new LineRender() { Text = $"1. Player White: {playerWhite}", Style = view.selectedIndex == 0 ? LineStyle.Selected : LineStyle.None });
            AddLine(new LineRender() { Text = $"2. Player Black: {playerBlack}", Style = view.selectedIndex == 1 ? LineStyle.Selected : LineStyle.None });
            AddLine(new LineRender() { Text = $"3. Ai level: {view.AiDifficulty}", Style = view.selectedIndex == 2 ? LineStyle.Selected : LineStyle.None });
        }
    }
}
