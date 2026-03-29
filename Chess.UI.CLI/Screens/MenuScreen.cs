using Chess.Application.Models;
using Chess.UI.CLI.Models;
using Chess.UI.CLI.Panels;
using Chess.UI.CLI.Views;

namespace Chess.UI.CLI.Screens
{
    // Главное меню
    public class MenuScreen : BaseScreen
    {      
        private MenuPanel _menuPanel;
        private MenuView _menuView;
        private readonly GameSettings _gameSettings;

        public MenuScreen(ScreenManager manager) : base(manager) {
            _menuPanel = new MenuPanel(0, 0, Console.WindowWidth, Console.WindowHeight);
            _menuView = new MenuView() { MenuItems = new List<string>() { "1. New Game", "2. Settings", "3. Quit" }, selectedIndex = 0 };
        }

        public override void Render()
        {
            _menuPanel.Render(_menuView);
        }

        public override bool HandleInput(PlayerAction action)
        {
            if (action == null) return false;

            switch (action.Type) {
                case PlayerActionType.MoveDown:
                    MoveDown();
                    break;
                case PlayerActionType.MoveUp:
                    MoveUp();
                    break;
                case PlayerActionType.Select:
                    HandleSelection();
                    break;
                
                default:
                    break;
            }
            return false;
        }

        public void MoveDown()
        {
            _menuView.selectedIndex++;
            if (_menuView.selectedIndex > _menuView.MenuItems.Count - 1)
                _menuView.selectedIndex = _menuView.MenuItems.Count - 1;
        }

        public void MoveUp()
        {
            _menuView.selectedIndex--;
            if (_menuView.selectedIndex < 0)
                _menuView.selectedIndex = 0;
        }

        private void HandleSelection()
        {
            switch (_menuView.selectedIndex) {
                case 0:
                    _manager.SetScreen(new GameScreen(_manager));
                    break;

                case 1:
                    _manager.SetScreen(new SettingsScreen(_manager, _manager.GameSettings));
                    break;

                case 2:
                    _manager.SetScreen(new QuitScreen(_manager));
                    break;
            }
        }
    }
}
