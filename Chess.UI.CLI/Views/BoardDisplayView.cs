using Chess.Application.Views;

namespace Chess.UI.CLI.Views
{
    //TODO перейти от DTO-моделей с setter к моделям с getter + constructor
    public class BoardDisplayView
    {
        public BoardView BoardView { get; set; }
        public bool IsBoardFlipped { get; set; }
    }
}
