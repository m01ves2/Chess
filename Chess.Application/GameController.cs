using Chess.Domain;
using Chess.Engine;

namespace Chess.Application
{
    public class GameController
    {
        private readonly GameScene _gameScene;
        private GamePosition _gamePosition;
        private readonly GameEngine _gameEngine;
        private bool _isGameOver = false;
        public GameScene CurrentGameState => _gameScene;

        private Stack<GameSnapshot> _SnapshotHistory = new Stack<GameSnapshot>();

        public GameController()
        {
            _gameScene = new GameScene();
            _gamePosition = new GamePosition(new Board(), new GameState(), PieceColor.White);
            _gameEngine = new GameEngine();

            UpdateGameScene();

            // добавляем стартовый snapshot в стек
            _SnapshotHistory.Push(CreateSnapshot());
        }
        // Обработка действия игрока
        public void ProcessAction(PlayerAction action)
        {
            if (action == null) return;

            switch (action.Type) {
                case PlayerActionType.MoveUp:
                    MoveCursor(-1, 0);
                    break;
                case PlayerActionType.MoveDown:
                    MoveCursor(1, 0);
                    break;
                case PlayerActionType.MoveLeft:
                    MoveCursor(0, -1);
                    break;
                case PlayerActionType.MoveRight:
                    MoveCursor(0, 1);
                    break;

                case PlayerActionType.Select:
                    Select(_gameScene.Cursor);
                    break;

                case PlayerActionType.NewGame:
                    // TODO: сброс игры
                    break;

                case PlayerActionType.Undo:
                    UndoMove();
                    break;
                case PlayerActionType.Quit:
                    _isGameOver = true;
                    break; // завершаем игру

                default:
                    break;
            }
        }

        private void Select(Position position)
        {
            var selectedSquare = _gamePosition.Board.GetSquare(position);
            // Сброс подсветки, если новая клетка выбрана
            _gameScene.HighlightedPositions.Clear();


            if (_gameScene.SelectedPosition == position) { //сняли выделение с выбраной клетки
                _gameScene.SelectedPosition = null;
                return;
            }
            
            if (selectedSquare.Piece?.Color == _gamePosition.CurrentPlayer) {//выделили клетку
                _gameScene.SelectedPosition = position;
                var moves = _gameEngine.GetLegalMoves(_gamePosition, position).ToList();
                moves.ForEach(m => _gameScene.HighlightedPositions.Add(m.To));
                return;
            }

            if (_gameScene.SelectedPosition != null) {
                TryMakeMove(_gameScene.SelectedPosition.Value, position);
                _gameScene.SelectedPosition = null;
            }
        }

        public void TryMakeMove(Position from, Position to)
        {
            var moves = _gameEngine.GetLegalMoves(_gamePosition, from);
            var move = moves.FirstOrDefault(m => m.To == to);

            if (move == null)
                return;

            DoMove(move);
        }


        private GameSnapshot CreateSnapshot()
        {
            var boardCopy = _gamePosition.Board.Clone();        // глубокая копия
            var stateCopy = _gamePosition.State.Clone();        // тоже копия
            var playerCopy = _gamePosition.CurrentPlayer;

            return new GameSnapshot(boardCopy, stateCopy, playerCopy);
        }

        private void RestoreSnapshot(GameSnapshot snapshot)
        {
            _gamePosition = snapshot.ToGamePosition();
        }

        public void DoMove(Move move)
        {
            _SnapshotHistory.Push(CreateSnapshot()); // snapshot ДО хода
            _gameEngine.MakeMove(_gamePosition, move);      // применяем ход один раз
            _gamePosition.MoveHistory.Add(move);
            SwitchPlayer();
            UpdateCheckStatus();
        }

        public void UndoMove()
        {
            _gameScene.SelectedPosition = null;
            _gameScene.HighlightedPositions.Clear();
            if (_SnapshotHistory.Count > 0) {
                var snapshot = _SnapshotHistory.Pop();
                
                if (_gamePosition.MoveHistory.Count > 0) {
                    _gamePosition.MoveHistory.Remove(_gamePosition.MoveHistory.Last());
                }

                //_gamePosition.Board = snapshot.Board;
                //_gameEngine.RestoreState(snapshot.State);
                //_gamePosition.CurrentPlayer = snapshot.CurrentPlayer;
                //_gamePosition.Restore(snapshot);
                RestoreSnapshot(snapshot);
                UpdateGameScene();
                //_gameEngine.UpdateBoard(_gameScene.Board);
            }
        }

        private void SwitchPlayer()
        {
            //_gameScene.CurrentPlayer = (_gameScene.CurrentPlayer == _gameScene.PlayerWhite) ?
            //               _gameScene.PlayerBlack : _gameScene.PlayerWhite;

            //if( _gameScene.CurrentPlayer == _gameScene.PlayerWhite.Color) {
            //    _gameScene.CurrentPlayer = _gameScene.PlayerBlack.Color;
            //    _gameScene.Cursor = new Position(2, 3);
            //}
            //else {
            //    _gameScene.CurrentPlayer = _gameScene.PlayerWhite.Color;
            //    _gameScene.Cursor = new Position(5, 4);
            //}

            _gamePosition.SwitchTurn();
        }

        private void MoveCursor(int dRow, int dCol)
        {
            var newPosition = new Position(_gameScene.Cursor.Row + dRow, _gameScene.Cursor.Col + dCol);
            if (_gamePosition.Board.IsInsideBoard(newPosition))
                _gameScene.Cursor = newPosition;
        }

        private void UpdateCheckStatus()
        {
            _gameScene.WhiteKingInCheck = _gameEngine.IsKingInCheck(_gamePosition, PieceColor.White);
            _gameScene.BlackKingInCheck = _gameEngine.IsKingInCheck(_gamePosition, PieceColor.Black);
        }

        public bool IsGameOver()
        {
            return _isGameOver;
        }

        public void UpdateGameScene()
        {
            _gameScene.Board = _gamePosition.Board;
            _gameScene.MoveHistory = _gamePosition.MoveHistory;

            _gameScene.WhiteKingInCheck = _gameEngine.IsKingInCheck(_gamePosition, PieceColor.White);
            _gameScene.BlackKingInCheck = _gameEngine.IsKingInCheck(_gamePosition, PieceColor.Black);
        }
    }
}
