using Chess.Application;
using Chess.UI.CLI.Models;

namespace Chess.UI.CLI.Screens.BaseScreens
{
    public abstract class BaseScreen
    {
        //protected const int MinWidth = 80;
        //protected const int MinHeight = 40;

        protected readonly ScreenManager _manager;
        private int ConsoleWidth = Console.WindowWidth;
        private int ConsoleHeight = Console.WindowHeight;

        public BaseScreen(ScreenManager manager)
        {
            _manager = manager;
            Init();
        }

        public abstract void Render();
        public abstract bool HandleInput(PlayerAction action);
    

        public void ConsoleResize()
        {
            if (Console.WindowHeight != ConsoleHeight || Console.WindowWidth != ConsoleWidth)
                Init();
        }

        protected virtual void Init()
        {
        }

        public virtual void Tick()
        {
            //To nothing
        }
    }
}
