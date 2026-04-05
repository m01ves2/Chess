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
            _quitPanel.SetView(_quitView);

            BuildPanels();
        }

        protected void BuildPanels()
        {
            _panels.Clear();
            _panels.Add(_quitPanel);
        }

        public override void BuildScreen()
        {
        }

        public override bool HandleInput(PlayerAction action)
        {
            _manager.RequestExit();
            return false;
        }

    }
}
