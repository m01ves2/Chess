using Chess.Application;

namespace Chess.UI.CLI.Screens
{
    public class QuitScreen : BaseScreen
    {
        public QuitScreen(ScreenManager manager) : base(manager)
        {
        }

        public override void HandleInput(PlayerAction action)
        {
            _manager.RequestExit();
        }

        public override void Render()
        {
            Console.Clear();
            Console.WriteLine("=== Chess CLI ===");
            Console.WriteLine("Press any key to quit...");

            //TODO show game statistics

        }
    }
}
