using Chess.Application;

namespace Chess.UI.CLI.Screens
{
    public class GameSettingsScreen : BaseScreen
    {
        private int selectedIndex = 0;
        private List<string> items => new List<string>() { 
            "1. Player White: " + _manager.GameSettings.WhitePlayer, 
            "2. Player Black: " + _manager.GameSettings.BlackPlayer, 
            "3. Ai level: " + _manager.GameSettings.AiDifficulty
        };

        public GameSettingsScreen(ScreenManager manager) : base(manager) { }


        public override void Render()
        {
            Console.Clear();
            Console.WriteLine("=== Chess CLI ===");
            for (int i = 0; i < items.Count; i++) {
                if (i == selectedIndex)
                    PrintSelected(items[i]);
                else
                    PrintNormal(items[i]);
            }
        }

        public override void HandleInput(PlayerAction action)
        {
            if (action == null) return;

            switch (action.Type) {
                case PlayerActionType.MoveDown:
                    selectedIndex++;
                    if (selectedIndex > items.Count - 1) selectedIndex = items.Count - 1;
                    break;
                case PlayerActionType.MoveUp:
                    selectedIndex--;
                    if (selectedIndex < 0) selectedIndex = 0;
                    break;
                case PlayerActionType.Select:
                    if (selectedIndex == 0) {
                        _manager.TogglePlayerWhiteSetting();
                    }
                    else if (selectedIndex == 1)
                        _manager.TogglePlayerBlackSetting();
                    else if (selectedIndex == 2) {
                        _manager.ToggleAiDifficultySetting();
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
