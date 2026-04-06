using Chess.Application.Models;

namespace Chess.UI.CLI.Views
{
    public class SettingsView
    {
        public PlayerType PlayerTypeWhite { get; set; }
        public PlayerType PlayerTypeBlack { get; set; }
        public int AiDifficulty { get; set; }
        public int SelectedIndex { get; set; } = 0;

        public List<string> Items => new(){
        $"1. Player White: {PlayerTypeWhite}",
        $"2. Player Black: {PlayerTypeBlack}",
        $"3. Ai level: {AiDifficulty}"
        };
    }
}
