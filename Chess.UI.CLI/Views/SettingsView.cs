using Chess.Application.Models;

namespace Chess.UI.CLI.Views
{
    public class SettingsView
    {
        //public List<string> SettingsItems = new List<string>();
        public PlayerType PlayerTypeWhite { get; set; }
        public PlayerType PlayerTypeBlack { get; set; }
        public int AiDifficulty { get; set; }  
        public int selectedIndex = 1;
    }
}
