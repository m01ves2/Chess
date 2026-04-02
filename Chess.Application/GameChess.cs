using Chess.Application.Models;
using Chess.Application.Players;
using Chess.Application.Views;
using Chess.Domain;

namespace Chess.Application
{
    public class GameChess
    {
        private GameController _controller;
        private IPlayer _whitePlayer;
        private IPlayer _blackPlayer;
        //private IPlayer _currentPlayer;

        private DateTime _lastMoveTime = DateTime.MinValue;
        private TimeSpan _aiDelay = TimeSpan.FromMilliseconds(300);

        private List<Move> _allLegalMovesCache = new List<Move>();
        public PieceViewColor Winner => _controller.Winner == PieceColor.White ? PieceViewColor.White : PieceViewColor.Black;
        public bool IsGameOver => _controller.IsGameOver;

        public GameChess(GameSettings gameSettings)
        {
            _controller = new GameController();

            if(gameSettings.WhitePlayer == PlayerType.Human)
                _whitePlayer = new HumanPlayer(_controller); //потом отдельно отрегулируем Ai va Ai
            else
                _whitePlayer = new AiPlayer(_controller, gameSettings.AiDifficulty);

            if (gameSettings.BlackPlayer == PlayerType.Human)
                _blackPlayer = new HumanPlayer(_controller); //потом отдельно отрегулируем Ai va Ai
            else
                _blackPlayer = new AiPlayer(_controller, gameSettings.AiDifficulty);
        }

        public void Tick()
        {
            if (GetCurrentPlayer() is AiPlayer) {
                if (DateTime.Now - _lastMoveTime < _aiDelay)
                    return;

                _lastMoveTime = DateTime.Now;
            }

            if(_allLegalMovesCache.Count == 0)
                _allLegalMovesCache.AddRange(_controller.GetAllLegalMoves().ToList());

            var moves = _allLegalMovesCache;

            if (!moves.Any())
                throw new Exception("No moves available"); //TODO GameOver??

            var move = GetCurrentPlayer().TryGetMove(moves);

            if (move != null) {
                _controller.DoMove(move);
                _allLegalMovesCache.Clear();
            }
        }

        public void Select(int row, int col)
        {
            if(GetCurrentPlayer() is HumanPlayer hp)
                hp.Select(row, col);
        }

        private IPlayer GetCurrentPlayer()
        {
            return _controller.GamePosition.CurrentPlayer == PieceColor.White ? _whitePlayer : _blackPlayer;
        }


        public BoardView GetBoardView()
        {
            var board = _controller.GetBoard(); // чистый домен

            Position? selection = null;
            if (GetCurrentPlayer() is HumanPlayer hp)
                selection = hp.SelectedFrom;

            List<Position> highlights = new List<Position>();
            highlights.AddRange(_allLegalMovesCache.Where(m => m.From == selection).Select(m => m.To).ToList());
            PieceColor currentPlayerColor = _controller.GamePosition.CurrentPlayer;

            return Mapper.GetBoardView(board, selection, highlights, currentPlayerColor);
        }

        public MoveHistoryView GetHistoryView()
        {
            var history = _controller.GetMoveHistory();
            return Mapper.GetHistoryView(history);

        }

        public InfoView GetInfoView()
        {
            //return Mapper.GetInfoView();
            return new InfoView() { };
        }

        public CapturedView GetCapturedView()
        {
            return new CapturedView() { };
        }

        public void UndoMove()
        {
            _controller.UndoMove();
        }
        

        //public InfoView GetInfoView()
        //{
        //    var infoView = new InfoView()
        //    {
        //        CurrentPlayer = (_gamePosition.CurrentPlayer == PieceColor.White ? PieceViewColor.White : PieceViewColor.Black),
        //        IsCheck = _gameScene.WhiteKingInCheck || _gameScene.BlackKingInCheck,
        //        IsPromoted = IsPromotionPending,
        //    };
        //    return infoView;
        //}

        //public CapturedView GetCapturedView()
        //{
        //    var capturedView = new CapturedView();
        //    foreach (var captured in _gamePosition.Board.WhiteCaptured) {
        //        capturedView.WhiteCaptured.Add(MapPieceType(captured));
        //    }
        //    foreach (var captured in _gamePosition.Board.BlackCaptured) {
        //        capturedView.BlackCaptured.Add(MapPieceType(captured));
        //    }
        //    return capturedView;
        //}
    }
}
