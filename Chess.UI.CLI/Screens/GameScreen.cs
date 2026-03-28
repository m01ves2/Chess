using Chess.Application;
using Chess.Domain;
using Chess.UI.CLI.Models;
using Chess.UI.CLI.Panels;

namespace Chess.UI.CLI.Screens
{
    public class GameScreen : BaseScreen
    {
        private readonly GameController _gameController;

        private BoardPanel _boardPanel;
        private HistoryPanel _historyPanel;
        private InfoPanel _infoPanel;
        private MessagePanel _messagePanel;

        private int ConsoleWidth = Console.WindowWidth;
        private int ConsoleHeight = Console.WindowHeight;

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

            var boardView = _gameController.GetBoardView();
            _boardPanel.Render(boardView);

            var historyView = _gameController.GetHistoryView();
            _historyPanel.Render(historyView);

            var infoView = _gameController.GetInfoView();
            _infoPanel.Render(infoView);

            var messageView = _gameController.GetMessageView();
            _messagePanel.Render(messageView);
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
    }
}
