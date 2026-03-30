using Chess.Application;
using Chess.Application.Models;
using Chess.Application.ViewModels;
using Chess.Domain;
using Chess.UI.CLI.Models;
using Chess.UI.CLI.Panels;
using Chess.UI.CLI.Screens.BaseScreens;
using Chess.UI.CLI.Views;
using System.Data;

namespace Chess.UI.CLI.Screens
{
    public class GameScreen : BaseScreen
    {
        private readonly GameController _gameController;

        private BoardPanel _boardPanel;
        bool isBoardFlipped = false;

        private HistoryPanel _historyPanel;
        
        private InfoPanel _infoPanel;
        
        private CapturedPanel _capturedPanel;

        
        private PromotionPanel _promotionPanel;
        //private BoardView _promotionView;
        private bool isPromotion = true;
        
        private GameOverPanel _gameOverPanel;
        private GameOverView _gameOverView;

        public GameScreen(ScreenManager manager) : base(manager)
        {
            _gameController = new GameController();
        }

        protected override void Init()
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

            _promotionPanel = new PromotionPanel(8, 12, 14, 5);
            _gameOverPanel = new GameOverPanel(5, 12, 20, 6 );

        }

        private BoardView RenderPromotionView()
        {
            var promotionView = new BoardView();
            promotionView.Cells = new CellView[1, 4];
            promotionView.Cells[0, 0] = new CellView() { PieceView = new PieceView() { Color = PieceViewColor.Black, Type = PieceViewType.Queen } };
            promotionView.Cells[0, 1] = new CellView() { PieceView = new PieceView() { Color = PieceViewColor.Black, Type = PieceViewType.Rook } };
            promotionView.Cells[0, 2] = new CellView() { PieceView = new PieceView() { Color = PieceViewColor.Black, Type = PieceViewType.Bishop } };
            promotionView.Cells[0, 3] = new CellView() { PieceView = new PieceView() { Color = PieceViewColor.Black, Type = PieceViewType.Knight } };
            //_promotionView.Cells[promotionCursor.Row, promotionCursor.Col].IsCursor = true;
            return promotionView;
        }

        private void RenderGameOveView()
        {
            _gameOverView = new GameOverView() { GameOverItems = new List<string>() { "G A M E  O V E R !", " ", $"{_gameController.Winner} side wins!" } };
        }

        public override void Render()
        {
            ConsoleResize();

            //bool rotate = _mode == GameMode.HumanVsHuman
            //        && _controller.CurrentPlayer == PieceColor.Black;

            var boardView = _gameController.GetBoardView();
            isBoardFlipped = (_manager.GameSettings.WhitePlayer == PlayerType.Human && boardView.currentPlayer == PieceViewColor.Black);
            _boardPanel.Render(new BoardDisplayView() { BoardView = boardView, IsBoardFlipped = isBoardFlipped });

            var historyView = _gameController.GetHistoryView();
            _historyPanel.Render(historyView);

            var infoView = _gameController.GetInfoView();
            _infoPanel.Render(infoView);

            var capturedView = _gameController.GetCapturedView();
            _capturedPanel.Render(capturedView);

            isPromotion = _gameController.IsPromotionPending;
            if (isPromotion) {
                var promotionView =  RenderPromotionView();
                _promotionPanel.Render(promotionView);
            }

            if (_gameController.IsGameOver()) {
                RenderGameOveView();
                _gameOverPanel.Render(_gameOverView);
            }
        }

        public override bool HandleInput(PlayerAction action)
        {
            if (action == null) return false;

            if (isPromotion) {
                return HandlePromotion(action);
            }

            switch (action.Type) {
                case PlayerActionType.MoveUp:
                    //_gameController.MoveCursor(-1, 0);
                    _boardPanel.MoveUp();
                    break;
                case PlayerActionType.MoveDown:
                    //_gameController.MoveCursor(1, 0);
                    _boardPanel.MoveDown();
                    break;
                case PlayerActionType.MoveLeft:
                    //_gameController.MoveCursor(0, -1);
                    _boardPanel.MoveLeft();
                    break;
                case PlayerActionType.MoveRight:
                    //_gameController.MoveCursor(0, 1);
                    _boardPanel.MoveRight();
                    break;

                case PlayerActionType.Select:
                    _gameController.Select(_boardPanel.boardCursor.Row, _boardPanel.boardCursor.Col);
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
            return false;
        }

        public bool HandlePromotion(PlayerAction action)
        {
            switch (action.Type) {
                case PlayerActionType.MoveLeft:
                    _promotionPanel.MoveLeft();
                    break;
                case PlayerActionType.MoveRight:
                    _promotionPanel.MoveRight();
                    break;
                case PlayerActionType.Select:
                    var promotionPiece =  _promotionPanel.Select();
                    _gameController.CompletePromotion(promotionPiece);
                    break;
                case PlayerActionType.Escape:
                    _gameController.CancelPromotion();
                    return true;
                    
                default:
                    break;
            }
            return false;
        }

    }
}
