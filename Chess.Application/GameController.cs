using Chess.Application.Models;
using Chess.Domain;
using Chess.Engine;
using System.Drawing;

namespace Chess.Application
{
    public enum GameResult
    {
        None,
        Checkmate,
        Stalemate,
    }

    public class GameController
    {
        public Guid Id { get; } = Guid.NewGuid();

        public GameInfo GameInfo { get; private set; }
        private readonly GameEngine _gameEngine;
        private GamePosition _gamePosition;

        public GamePosition GamePosition => _gamePosition;


        private List<Move> _moveHistory { get; set; } = new List<Move>();

        private Stack<GameSnapshot> _snapshotHistory = new Stack<GameSnapshot>();


        private bool _isGameOver = false;
        public bool IsGameOver => _isGameOver;
        public PieceColor? Winner { get; private set; }

        public GameResult Result { get; private set; } = GameResult.None;

        public GameController()
        {
            //Console.WriteLine($"Controller ID: {Id}");
            //System.Diagnostics.Debug.WriteLine($"Controller ID: {Id}");

            GameInfo = new GameInfo();
            _gamePosition = new GamePosition(new Board(), new GameState(), PieceColor.White);
            _gameEngine = new GameEngine();

            // добавляем стартовый snapshot в стек
            _snapshotHistory.Push(CreateSnapshot());
        }

        public void DoMove(Move move, bool updateGameInfo = true)
        {
            if (move == null) return;

            _snapshotHistory.Push(CreateSnapshot()); // snapshot ДО хода
            _gameEngine.MakeMove(_gamePosition, move);      // применяем ход один раз
            _moveHistory.Add(move);
            SwitchPlayer();

            if (updateGameInfo) {
                UpdateGameInfo();


                //if (_gameEngine.IsCheckmate(_gamePosition, _gamePosition.CurrentPlayerColor)) {
                //    _isGameOver = true;
                //    Winner = _gamePosition.CurrentPlayerColor == PieceColor.White
                //        ? PieceColor.Black
                //        : PieceColor.White;
                //    Result = GameResult.Checkmate;
                //}
                //else if (_gameEngine.IsStalemate(_gamePosition, _gamePosition.CurrentPlayerColor)) {
                //    _isGameOver = true;
                //    Winner = null; // ничья
                //    Result = GameResult.Stalemate;
                //}
            }

        }

        public void UndoMove(bool updateGameInfo = true)
        {
            if (_snapshotHistory.Count > 1 && _moveHistory.Count > 0) {
                var snapshot = _snapshotHistory.Pop();
                _moveHistory.Remove(_moveHistory.Last());

                RestoreSnapshot(snapshot);
                if (updateGameInfo) {
                    UpdateGameInfo();
                }
            }
        }

        private void SwitchPlayer()
        {
            _gamePosition.SwitchTurn();
        }

        private GameSnapshot CreateSnapshot()
        {
            var boardCopy = _gamePosition.Board.Clone();        // глубокая копия
            var stateCopy = _gamePosition.State.Clone();        // тоже копия
            var playerCopy = _gamePosition.CurrentPlayerColor;

            return new GameSnapshot(boardCopy, stateCopy, playerCopy);
        }

        private void RestoreSnapshot(GameSnapshot snapshot)
        {
            _gamePosition = snapshot.ToGamePosition();
        }

        public void UpdateGameInfo()
        {
            GameInfo.WhiteKingInCheck = false;
            GameInfo.BlackKingInCheck = false;

            var kingInCheck = IsKingInCheck(_gamePosition, _gamePosition.CurrentPlayerColor);
            if (kingInCheck) {
                if (_gamePosition.CurrentPlayerColor == PieceColor.White) {
                    GameInfo.WhiteKingInCheck = true;
                }
                else {
                    GameInfo.BlackKingInCheck = true;
                }
            }

            CheckmateOrStalemate(kingInCheck);
        }

        public void CheckmateOrStalemate(bool kingInCheck)
        {
            _isGameOver = false;
            Winner = null;
            Result = GameResult.None;

            var moves = _gameEngine.GetAllLegalMoves(_gamePosition, _gamePosition.CurrentPlayerColor);
            if (!moves.Any()) {
                _isGameOver = true;
                if (kingInCheck) {
                    Winner = _gamePosition.CurrentPlayerColor == PieceColor.White
                        ? PieceColor.Black
                        : PieceColor.White;
                    Result = GameResult.Checkmate;
                }
                else {
                    Winner = null;
                    Result = GameResult.Stalemate;
                }
            }
        }

        //public bool IsCheckmate(GamePosition gamePosition, PieceColor color)
        //{
        //    if (!IsKingInCheck(gamePosition, color))
        //        return false;

        //    var moves = _gameEngine.GetAllLegalMoves(gamePosition, color);
        //    return !moves.Any();
        //}

        public bool IsKingInCheck(GamePosition gamePosition, PieceColor color )
        {
            return _gameEngine.IsKingInCheck(gamePosition, color);
        }

        public IEnumerable<Move> GetAllLegalMoves()
        {
            return _gameEngine.GetAllLegalMoves(_gamePosition, _gamePosition.CurrentPlayerColor);
        }

        public Board GetBoard() => _gamePosition.Board;
        public IReadOnlyList<Move> GetMoveHistory() => _moveHistory;
    }
}
