using Chess.UI.CLI.Models;
using Chess.UI.CLI.Panels;
using Chess.UI.CLI.Screens.BaseScreens;
using Chess.UI.CLI.Views;

namespace Chess.UI.CLI.Screens
{
    public class QuitScreen : BaseScreen
    {
        private QuitPanel _quitPanel;
        private QuitView _quitView;
        public QuitScreen(ScreenManager manager) : base(manager)
        {
            _quitPanel = new QuitPanel(0, 0, Console.WindowWidth, Console.WindowHeight);
            _quitView = new QuitView() { QuitItems = new List<string>() { "Press any key to continue..." } };
        }
        public override void Render()
        {
            _quitPanel.Render(_quitView);
        }

        public override bool HandleInput(PlayerAction action)
        {
            _manager.RequestExit();
            return false;
        }

    }
}
