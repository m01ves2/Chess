using Chess.Application;
using Chess.Domain;
using Chess.UI.CLI.Panels;

namespace Chess.UI.CLI.Screens
{
    public class GameScreen : BaseScreen
    {
        private GameController _gameController;

        private BoardPanel _boardPanel;
        private HistoryPanel _historyPanel;
        private InfoPanel _infoPanel;
        private MessagePanel _messagePanel;

        private int ConsoleWidth = Console.WindowWidth;
        private int ConsoleHeight = Console.WindowHeight;

        //private bool _isTooSmall = false;

        public GameScreen(ScreenManager manager) : base(manager)
        {
            Init();
            _gameController = new GameController();
        }

        private void Init()
        {
            int leftPanelWidth = 30;
            int rightPanelWidth = Console.WindowWidth - leftPanelWidth;
            int boardPanelHeight = 30;
            int infoPanelHeight = 4;

            _boardPanel = new BoardPanel(0, 0, leftPanelWidth, boardPanelHeight);
            _messagePanel = new MessagePanel(0, boardPanelHeight, leftPanelWidth, Console.WindowHeight - boardPanelHeight);
            _infoPanel = new InfoPanel(leftPanelWidth, 0, rightPanelWidth, infoPanelHeight);
            _historyPanel = new HistoryPanel(leftPanelWidth, infoPanelHeight, rightPanelWidth, Console.WindowHeight - infoPanelHeight);
        }

        public override void Render()
        {
            ConsoleResize();

            //bool rotate = _mode == GameMode.HumanVsHuman
            //        && _controller.CurrentPlayer == PieceColor.Black;

            //var boardVM = _controller.GetBoardView(rotate);

            //_boardPanel.SetData(boardVM);
            _boardPanel.Render();

            var historyVM = _gameController.GetHistoryView();
            _historyPanel.SetData(historyVM);
            _historyPanel.Render();

            var infoVM = _gameController.GetInfoView();
            _infoPanel.SetData(infoVM);
            _infoPanel.Render();
            
            _messagePanel.Render();
        }

        public override void HandleInput(PlayerAction action)
        {
            if (action == null) return;

            switch (action.Type) {
                case PlayerActionType.MoveUp:
                    _gameController.MoveCursor(-1, 0);
                    break;
                case PlayerActionType.MoveDown:
                    _gameController.MoveCursor(1, 0);
                    break;
                case PlayerActionType.MoveLeft:
                    _gameController.MoveCursor(0, -1);
                    break;
                case PlayerActionType.MoveRight:
                    _gameController.MoveCursor(0, 1);
                    break;

                case PlayerActionType.Select:
                    _gameController.Select(_gameController.Scene.Cursor);
                    break;

                case PlayerActionType.Undo:
                    _gameController.UndoMove();
                    break;
                case PlayerActionType.NewGame:
                    _manager.SetScreen(new GameScreen(_manager));
                    break;

                default:
                    break;
            }
        }

        public void ConsoleResize()
        {
            //int w = Console.WindowWidth;
            //int h = Console.WindowHeight;

            //bool tooSmall = w < MinWidth || h < MinHeight;

            //if (tooSmall) {
            //    if (!_isTooSmall) {
            //        _isTooSmall = true;
            //        Console.Clear();
            //    }

            //    ShowResizeWarning();
            //    return;
            //}

            //// если только что восстановились
            //if (_isTooSmall) {
            //    _isTooSmall = false;
            //    RebuildLayout();
            //    return;
            //}

            // обычный resize
            //if (w != ConsoleWidth || h != ConsoleHeight) {
            //    ConsoleWidth = w;
            //    ConsoleHeight = h;
            //    RebuildLayout();
            //}

            if (Console.WindowHeight != ConsoleHeight || Console.WindowWidth != ConsoleWidth)
                Init();
        }

        //private void ShowResizeWarning()
        //{
        //    Console.Clear();
        //    Console.SetCursorPosition(0, 0);
        //    Console.WriteLine("Window too small!");
        //    Console.WriteLine($"Minimum: {MinWidth}x{MinHeight}");
        //    Console.WriteLine($"Current: {Console.WindowWidth}x{Console.WindowHeight}");
        //}

        //private void RebuildLayout()
        //{
        //    // ВАЖНО: сначала очистка
        //    Console.Clear();

        //    int leftPanelWidth = 30;
        //    int rightPanelWidth = ConsoleWidth - leftPanelWidth;
        //    int boardPanelHeight = 30;
        //    int infoPanelHeight = 4;

        //    _boardPanel = new BoardPanel(0, 0, leftPanelWidth, boardPanelHeight);
        //    _messagePanel = new MessagePanel(0, boardPanelHeight, leftPanelWidth, ConsoleHeight - boardPanelHeight);
        //    _infoPanel = new InfoPanel(leftPanelWidth, 0, rightPanelWidth, infoPanelHeight);
        //    _historyPanel = new HistoryPanel(leftPanelWidth, infoPanelHeight, rightPanelWidth, ConsoleHeight - infoPanelHeight);
        //}
    }
}
