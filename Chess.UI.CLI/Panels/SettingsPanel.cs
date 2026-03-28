using Chess.UI.CLI.Panels.BasePanels;
using Chess.UI.CLI.Panels.BasePanels.Rendering;
using Chess.UI.CLI.Views;

namespace Chess.UI.CLI.Panels
{
    public class SettingsPanel : TextPanelBase<SettingsView>
    {
        //private int selectedIndex = 0;
        //private List<string> _lines => new List<string>() {
        //    "1. Player White: " + _gameSettings.WhitePlayer,
        //    "2. Player Black: " + _gameSettings.BlackPlayer,
        //    "3. Ai level: " + _gameSettings.AiDifficulty
        //};
        
        public SettingsPanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }

        public override void BuildBuffer(SettingsView view)
        {
            ClearLines();

            for (int i = 0; i < view.SettingsItems.Count; i++) {
                var menuItem = view.SettingsItems[i];
                AddLine(new LineRender() { Text = menuItem, Style = (i == view.selectedIndex ? LineStyle.Selected : LineStyle.None) });
            }
        }
    }
}
