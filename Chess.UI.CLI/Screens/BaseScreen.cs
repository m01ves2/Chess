using Chess.Application;

namespace Chess.UI.CLI.Screens
{
    public abstract class BaseScreen
    {
        //protected const int MinWidth = 80;
        //protected const int MinHeight = 40;
        
        protected ScreenManager _manager;

        public BaseScreen(ScreenManager manager)
        {
            _manager = manager;
        }

        public abstract void Render();
        public abstract void HandleInput(PlayerAction action);

        protected void PrintSelected(string item)
        {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(item);
            Console.ForegroundColor = defaultColor;
        }

        protected void PrintNormal(string item)
        {
            Console.WriteLine(item);
        }
    }
}
