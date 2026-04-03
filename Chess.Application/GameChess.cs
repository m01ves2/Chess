using Chess.Application.Models;
using Chess.Application.Players;
using Chess.Application.Views;
using Chess.Domain;
using System.Collections.Generic;
using System.Diagnostics;

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
        public bool IsPromotionPending => (GetCurrentPlayer() is HumanPlayer hp) ? hp.IsPromotionPending : false;

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
            System.Diagnostics.Debug.WriteLine($"Promotion: {IsPromotionPending}");

            if (GetCurrentPlayer() is AiPlayer) {
                if (DateTime.Now - _lastMoveTime < _aiDelay)
                    return;

                _lastMoveTime = DateTime.Now;
            }

            if(_allLegalMovesCache.Count == 0)
            //_allLegalMovesCache.Clear();
                _allLegalMovesCache.AddRange(_controller.GetAllLegalMoves().ToList());

            var moves = _allLegalMovesCache;

            if (!moves.Any())
                //throw new Exception("No moves available"); //TODO GameOver??
                return;

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
            return _controller.GamePosition.CurrentPlayerColor == PieceColor.White ? _whitePlayer : _blackPlayer;
        }

        public void UndoMove()
        {
            if (GetCurrentPlayer() is HumanPlayer hp)
                hp.UndoMove();
            _controller.UndoMove();
            _allLegalMovesCache.Clear();
        }

        public void CompletePromotion(PieceViewType promotionPiece)
        {
            if (GetCurrentPlayer() is HumanPlayer hp) {
                hp.CompletePromotion(promotionPiece);
            }
        }

        public void CancelPromotion()
        {
            if (GetCurrentPlayer() is HumanPlayer hp) {
                hp.CancelPromotion();
            }
        }

        public BoardView GetBoardView()
        {
            var board = _controller.GetBoard(); // чистый домен

            Position? selection = null;
            if (GetCurrentPlayer() is HumanPlayer hp)
                selection = hp.SelectedFrom;

            List<Position> highlights = new List<Position>();
            highlights.AddRange(_allLegalMovesCache.Where(m => m.From == selection).Select(m => m.To).ToList());
            PieceColor currentPlayerColor = _controller.GamePosition.CurrentPlayerColor;

            return Mapper.GetBoardView(board, selection, highlights, currentPlayerColor);
        }

        public MoveHistoryView GetHistoryView()
        {
            var history = _controller.GetMoveHistory();
            return Mapper.GetHistoryView(history);

        }

        public InfoView GetInfoView()
        {
            bool isCheck = _controller.GameInfo.BlackKingInCheck || _controller.GameInfo.WhiteKingInCheck;
            PieceColor currentPlayerColor = _controller.GamePosition.CurrentPlayerColor;

            return Mapper.GetInfoView( currentPlayerColor, isCheck, IsPromotionPending);
        }

        public CapturedView GetCapturedView()
        {
            List<Piece> whiteCaptured = _controller.GamePosition.Board.WhiteCaptured;
            List<Piece> blackCaptured = _controller.GamePosition.Board.BlackCaptured;
            return Mapper.GetCapturedView(whiteCaptured, blackCaptured);
        }
    }
}
