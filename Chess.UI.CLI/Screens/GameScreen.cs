using Chess.Application;
using Chess.Application.Models;
using Chess.Application.Views;
using Chess.UI.CLI.Models;
using Chess.UI.CLI.Panels;
using Chess.UI.CLI.Screens.BaseScreens;
using Chess.UI.CLI.Views;

namespace Chess.UI.CLI.Screens
{
    public class GameScreen : BaseScreen
    {
        private GameChess _gameChess;

        private BoardPanel _boardPanel;
        bool isBoardFlipped = false;

        private HistoryPanel _historyPanel;
        private InfoPanel _infoPanel;
        private CapturedPanel _capturedPanel;       
        private PromotionPanel _promotionPanel;
        private GameOverPanel _gameOverPanel;
        private bool _isPromotion = false;
        private bool _isGameOver = false;


        public GameScreen(ScreenManager manager, GameSettings gameSettings) : base(manager)
        {
            _gameChess = new GameChess(/*gameController, */gameSettings);

            int leftPanelWidth = 30;
            int rightPanelWidth = Console.WindowWidth - leftPanelWidth;
            int boardPanelHeight = 30;
            int infoPanelHeight = 6;

            _boardPanel = new BoardPanel(0, 0, leftPanelWidth, boardPanelHeight);
            _capturedPanel = new CapturedPanel(0, boardPanelHeight, leftPanelWidth, Console.WindowHeight - boardPanelHeight);
            _infoPanel = new InfoPanel(leftPanelWidth, 0, rightPanelWidth, infoPanelHeight);
            _historyPanel = new HistoryPanel(leftPanelWidth, infoPanelHeight, rightPanelWidth, Console.WindowHeight - infoPanelHeight);
            _promotionPanel = new PromotionPanel(8, 12, 14, 5);
            _gameOverPanel = new GameOverPanel(5, 12, 20, 6);

            BuildPanels();
        }

        protected void BuildPanels()
        {
            _panels.Clear();

            // сначала базовые панели
            _panels.Add(_boardPanel);
            _panels.Add(_capturedPanel);
            _panels.Add(_infoPanel);
            _panels.Add(_historyPanel);

            // только если активна — overlay
            if (_isPromotion)
                _panels.Add(_promotionPanel);
            if(_isGameOver)
                _panels.Add(_gameOverPanel);
        }


        public override void Tick()
        {
            _gameChess.Tick();
        }

        //protected override void Init()
        //{
        //    //Console.Clear();

        //    int leftPanelWidth = 30;
        //    int rightPanelWidth = Console.WindowWidth - leftPanelWidth;
        //    int boardPanelHeight = 30;
        //    int infoPanelHeight = 6;

        //    _boardPanel = new BoardPanel(0, 0, leftPanelWidth, boardPanelHeight);
        //    _capturedPanel = new CapturedPanel(0, boardPanelHeight, leftPanelWidth, Console.WindowHeight - boardPanelHeight);
        //    _infoPanel = new InfoPanel(leftPanelWidth, 0, rightPanelWidth, infoPanelHeight);
        //    _historyPanel = new HistoryPanel(leftPanelWidth, infoPanelHeight, rightPanelWidth, Console.WindowHeight - infoPanelHeight);
        //    _promotionPanel = new PromotionPanel(8, 12, 14, 5);
        //    _gameOverPanel = new GameOverPanel(5, 12, 20, 6 );
        //}

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
        }//TODO
        private GameOverView RenderGameOverView()
        {
            return new GameOverView() { GameOverItems = new List<string>() { "G A M E  O V E R !", " ", $"It's {_gameChess.Result}!",  $"{_gameChess.Winner} side wins!" } };
        }//TODO

        public override void BuildScreen()
        {
            if (CheckPromotionChanged()) {
                BuildPanels();
            }
            if(CheckGameOverChanged()) {
                BuildPanels();
            }

            var boardView = _gameChess.GetBoardView();
            isBoardFlipped = (_manager.GameSettings.BlackPlayer == PlayerType.Human && boardView.currentPlayer == PieceViewColor.Black);
            var boardDisplayView = new BoardDisplayView() { BoardView = boardView, IsBoardFlipped = isBoardFlipped };
            _boardPanel.SetView(boardDisplayView);

            var historyView = _gameChess.GetHistoryView();
            _historyPanel.SetView(historyView);

            var infoView = _gameChess.GetInfoView();
            _infoPanel.SetView(infoView);

            var capturedView = _gameChess.GetCapturedView();
            _capturedPanel.SetView(capturedView );


             var promotionView =  RenderPromotionView();
            _promotionPanel.SetView(promotionView);

             var gameOverView = RenderGameOverView();
            _gameOverPanel.SetView(gameOverView);
        }

        public bool CheckPromotionChanged()
        {
            if(_isPromotion != _gameChess.IsPromotionPending) {
                _isPromotion = _gameChess.IsPromotionPending;
                return true;
            }
            return false;
        }

        public bool CheckGameOverChanged()
        {
            if(_isGameOver != _gameChess.IsGameOver) {
                _isGameOver = _gameChess.IsGameOver;
                return true;
            }
            return false;
        }

        public override bool HandleInput(PlayerAction action)
        {
            if (action == null) return false;

            if (_isPromotion) {
                return HandlePromotion(action);
            }

            switch (action.Type) {
                case PlayerActionType.MoveUp:
                    _boardPanel.MoveUp();
                    break;
                case PlayerActionType.MoveDown:
                    _boardPanel.MoveDown();
                    break;
                case PlayerActionType.MoveLeft:
                    _boardPanel.MoveLeft();
                    break;
                case PlayerActionType.MoveRight:
                    _boardPanel.MoveRight();
                    break;

                case PlayerActionType.Select:
                    _gameChess.Select(_boardPanel.boardCursor.Row, _boardPanel.boardCursor.Col);
                    break;

                case PlayerActionType.Undo:
                    _gameChess.UndoMove();
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
                    _gameChess.CompletePromotion(promotionPiece);
                    break;
                case PlayerActionType.Escape:
                    _gameChess.CancelPromotion();
                    return true;
                    
                default:
                    break;
            }
            return false;
        }

    }
}
