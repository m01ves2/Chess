using Chess.UI.CLI.Panels.BasePanels;
using Chess.UI.CLI.Panels.BasePanels.Rendering;
using Chess.UI.CLI.Views;

namespace Chess.UI.CLI.Panels
{
    public class MenuPanel : TextPanelBase<MenuView>
    {
        //private int selectedIndex = 0;

        public MenuPanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }

        public override void BuildBuffer(MenuView view)
        {
            ClearLines();

            for(int i = 0; i < view.MenuItems.Count; i++) {
                var menuItem = view.MenuItems[i];
                AddLine(new LineRender() { Text = menuItem, Style = (i == view.selectedIndex ? LineStyle.Selected : LineStyle.None)});
            }
        }
    }
}
