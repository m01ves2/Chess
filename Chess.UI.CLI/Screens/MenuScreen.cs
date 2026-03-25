using Chess.Application;
using System.Runtime;

namespace Chess.UI.CLI.Screens
{
    // Главное меню
    public class MenuScreen : BaseScreen
    {
        private int selectedIndex = 0;
        private List<string> items = new List<string>() { "1. New Game", "2. Settings", "3. Quit" };

        public MenuScreen(ScreenManager manager) : base(manager) { }

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
                        _manager.SetScreen(new GameScreen(_manager));
                    }
                    else if (selectedIndex == 1)
                        _manager.SetScreen(new GameSettingsScreen(_manager));
                    else if (selectedIndex == 2) {
                        _manager.SetScreen(new QuitScreen(_manager));
                    }
                    break;
                
                default:
                    break;
            }
        }

        private void PrintSelected(string item)
        {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(item);
            Console.ForegroundColor = defaultColor;
        }

        private void PrintNormal(string item)
        {
            Console.WriteLine(item);
        }
    }
}
