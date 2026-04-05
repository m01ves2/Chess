using Chess.Application;
using Chess.Application.Models;
using Chess.UI.CLI.Models;
using Chess.UI.CLI.Panels;
using Chess.UI.CLI.Screens.BaseScreens;
using Chess.UI.CLI.Views;

namespace Chess.UI.CLI.Screens
{
    public class SettingsScreen : BaseScreen
    {
        //private int selectedIndex = 0;
        //private List<string> items => new List<string>() {
        //    "1. Player White: " + _manager.GameSettings.WhitePlayer,
        //    "2. Player Black: " + _manager.GameSettings.BlackPlayer,
        //    "3. Ai level: " + _manager.GameSettings.AiDifficulty
        //};

        private readonly GameSettings _gameSettings;
        private SettingsPanel _settingsPanel;
        private SettingsView _settingsView;
        public SettingsScreen(ScreenManager manager, GameSettings gameSettings) : base(manager)
        {
            _gameSettings = gameSettings;
            _settingsView = new SettingsView()
            {
                AiDifficulty    = _gameSettings.AiDifficulty,
                PlayerTypeBlack = _gameSettings.BlackPlayer,
                PlayerTypeWhite = _gameSettings.WhitePlayer,
                selectedIndex = 0,   
            };
            _settingsPanel = new SettingsPanel(0, 0, Console.WindowWidth, Console.WindowHeight);
            _settingsPanel.SetView(_settingsView);
            BuildPanels();
        }

        protected void BuildPanels()
        {
            _panels.Clear();
            _panels.Add(_settingsPanel);
        }

        public override void BuildScreen()
        {
            //_settingsPanel.SetView(_settingsView);
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
            _settingsView.selectedIndex++;
            if (_settingsView.selectedIndex > 2)
                _settingsView.selectedIndex = 2;
        }

        public void MoveUp()
        {
            _settingsView.selectedIndex--;
            if (_settingsView.selectedIndex < 0)
                _settingsView.selectedIndex = 0;
        }

        private void HandleSelection()
        {
            switch (_settingsView.selectedIndex) {
                case 0:
                    TogglePlayerWhiteSetting();
                    break;

                case 1:
                    TogglePlayerBlackSetting();
                    break;

                case 2:
                    ToggleAiDifficultySetting();
                    break;
            }
        }

        public void TogglePlayerWhiteSetting()
        {
            if (_gameSettings.WhitePlayer == PlayerType.Human)
                _gameSettings.WhitePlayer = PlayerType.Ai;
            else
                _gameSettings.WhitePlayer = PlayerType.Human;

            _settingsView.PlayerTypeWhite = _gameSettings.WhitePlayer;
        }

        public void TogglePlayerBlackSetting()
        {
            if (_gameSettings.BlackPlayer == PlayerType.Human)
                _gameSettings.BlackPlayer = PlayerType.Ai;
            else
                _gameSettings.BlackPlayer = PlayerType.Human;

            _settingsView.PlayerTypeBlack = _gameSettings.BlackPlayer;
        }

        public void ToggleAiDifficultySetting()
        {
            _gameSettings.AiDifficulty = _gameSettings.AiDifficulty % 3 + 1;
            _settingsView.AiDifficulty = _gameSettings.AiDifficulty;
        }
    }
}
