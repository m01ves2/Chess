using Chess.Application;
using Chess.Application.ViewModels;
using Chess.Domain;
using Chess.UI.CLI.Models;
using Chess.UI.CLI.Panels;
using Chess.UI.CLI.Views;

namespace Chess.UI.CLI.Screens
{
    public class GameScreen : BaseScreen
    {
        private readonly GameController _gameController;

        private BoardPanel _boardPanel;
        private HistoryPanel _historyPanel;
        private InfoPanel _infoPanel;
        private CapturedPanel _capturedPanel;
        
        private PromotionPanel _promotionPanel;
        private BoardView _promotionView;
        private bool isPromotion = true;
        private int promotionCursorCol = 0;

        private int ConsoleWidth = Console.WindowWidth;
        private int ConsoleHeight = Console.WindowHeight;

        public GameScreen(ScreenManager manager) : base(manager)
        {
            Init();
            _gameController = new GameController();
        }

        private void Init()
        {
            Console.Clear();

            int leftPanelWidth = 30;
            int rightPanelWidth = Console.WindowWidth - leftPanelWidth;
            int boardPanelHeight = 30;
            int infoPanelHeight = 6;

            _boardPanel = new BoardPanel(0, 0, leftPanelWidth, boardPanelHeight);
            _capturedPanel = new CapturedPanel(0, boardPanelHeight, leftPanelWidth, Console.WindowHeight - boardPanelHeight);
            _infoPanel = new InfoPanel(leftPanelWidth, 0, rightPanelWidth, infoPanelHeight);
            _historyPanel = new HistoryPanel(leftPanelWidth, infoPanelHeight, rightPanelWidth, Console.WindowHeight - infoPanelHeight);
            //_historyPanel = new HistoryPanel(leftPanelWidth, infoPanelHeight, rightPanelWidth, 7);

            _promotionPanel = new PromotionPanel(8, 12, 14, 5);
        }

        private void RenderPromotionView()
        {
            _promotionView = new BoardView();
            _promotionView.Cells = new CellView[1, 4];
            _promotionView.Cells[0, 0] = new CellView() { PieceView = new PieceView() { Color = PieceViewColor.Black, Type = PieceViewType.Queen } };
            _promotionView.Cells[0, 1] = new CellView() { PieceView = new PieceView() { Color = PieceViewColor.Black, Type = PieceViewType.Rook } };
            _promotionView.Cells[0, 2] = new CellView() { PieceView = new PieceView() { Color = PieceViewColor.Black, Type = PieceViewType.Bishop } };
            _promotionView.Cells[0, 3] = new CellView() { PieceView = new PieceView() { Color = PieceViewColor.Black, Type = PieceViewType.Knight } };
            _promotionView.Cells[0, promotionCursorCol].IsCursor = true;
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

            var capturedView = _gameController.GetCapturedView();
            _capturedPanel.Render(capturedView);

            isPromotion = _gameController.IsPromotionPending;
            if (isPromotion) {
                RenderPromotionView();
                _promotionPanel.Render(_promotionView);
            }
        }

        public override void HandleInput(PlayerAction action)
        {
            if (action == null) return;

            if (isPromotion) {
                HandlePromotion(action);
                return;
            }

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

                case PlayerActionType.PageUp:
                    _historyPanel.PageUp();
                    break;
                case PlayerActionType.PageDown:
                    _historyPanel.PageDown();
                    break;
                default:
                    break;
            }
        }

        public void HandlePromotion(PlayerAction action)
        {
            switch (action.Type) {
                case PlayerActionType.MoveLeft:
                    if (promotionCursorCol > 0)
                        promotionCursorCol--;
                    break;
                case PlayerActionType.MoveRight:
                    if (promotionCursorCol < _promotionView.Cells.Length - 1)
                        promotionCursorCol++;
                    break;
                case PlayerActionType.Select:
                    //TODO select promotion
                    var promotionPiece = _promotionView.Cells[0, promotionCursorCol].PieceView.Type;
                    _gameController.CompletePromotion(promotionPiece);
                    break;
                case PlayerActionType.Escape:
                    //TODO cancel promotion 

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
