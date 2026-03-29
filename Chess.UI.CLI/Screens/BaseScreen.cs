using Chess.Application;
using Chess.UI.CLI.Models;

namespace Chess.UI.CLI.Screens
{
    public abstract class BaseScreen
    {
        //protected const int MinWidth = 80;
        //protected const int MinHeight = 40;
        
        protected readonly ScreenManager _manager;

        public BaseScreen(ScreenManager manager)
        {
            _manager = manager;
        }

        public abstract void Render();
        public abstract void HandleInput(PlayerAction action);
    }
}
