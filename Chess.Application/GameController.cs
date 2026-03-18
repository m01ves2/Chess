using Chess.Application.Interfaces;
using Chess.Domain;
using Chess.Engine;
using Chess.Engine.Results;

namespace Chess.Application
{
    //Управлять состоянием игры: вызывать методы Game для совершения ходов, проверять правила.
    //Обеспечивать API для GameLoop: предоставлять текущую доску, подсвеченные возможные ходы, текущего игрока и т.д.
    public class GameController
    {
        private readonly GameScene _gameScene;
        private readonly GameEngine _gameEngine;
        private bool _isGameOver = false;
        public GameScene CurrentGameState => _gameScene;
        private Stack<GameSnapshot> _history = new Stack<GameSnapshot>();

        public GameController(GameScene gameScene)
        {
            _gameScene = gameScene;
            _gameEngine = new GameEngine(_gameScene.Board);

            // добавляем стартовый snapshot в стек
            _history.Push(CreateSnapshot());
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
            var selectedSquare = _gameScene.Board.GetSquare(position);
            // Сброс подсветки, если новая клетка выбрана
            _gameScene.HighlightedPositions.Clear();


            if (_gameScene.SelectedPosition == position) { //сняли выделение с выбраной клетки
                _gameScene.SelectedPosition = null;
                return;
            }
            
            if (selectedSquare.Piece?.Color == _gameScene.CurrentPlayer) {//выделили клетку
                _gameScene.SelectedPosition = position;
                _gameScene.HighlightedPositions.AddRange(_gameEngine.GetLegalMoves(position));
                return;
            }

            if (_gameScene.SelectedPosition != null) {
                TryMakeMove((Position)_gameScene.SelectedPosition!, position);
            }
        }

        public void TryMakeMove(Position from, Position to)
        {

            var result = _gameEngine.TryMove(from, to);
            if (result.Status == ResultStatus.Success) {
                var piece = _gameScene.Board.GetSquare(from).Piece!;
                var capturedPiece = result.CapturedPiece;
                var move = new Move(from, to, piece, capturedPiece);
                DoMove(move); // snapshot делаем здесь, MakeMove вызывается здесь один раз
            }
            // Сбрасываем выделение независимо от результата
            _gameScene.SelectedPosition = null;
        }


        private GameSnapshot CreateSnapshot()
        {
            var boardCopy = _gameScene.Board.Clone();        // глубокая копия
            var stateCopy = _gameEngine.State.Clone();        // тоже копия
            var playerCopy = _gameScene.CurrentPlayer;

            return new GameSnapshot(boardCopy, stateCopy, playerCopy);
        }

        public void DoMove(Move move)
        {
            _history.Push(CreateSnapshot()); // snapshot ДО хода
            _gameEngine.MakeMove(move);      // применяем ход один раз
            _gameScene.MoveHistory.Add(move);
            SwitchPlayer();
        }

        public void UndoMove()
        {
            _gameScene.SelectedPosition = null;
            _gameScene.HighlightedPositions.Clear();
            if (_history.Count > 0) {
                var snapshot = _history.Pop();

                _gameScene.Board = snapshot.Board;
                _gameEngine.UpdateBoard(_gameScene.Board);
                _gameEngine.State = snapshot.State;
                _gameScene.CurrentPlayer = snapshot.CurrentPlayer;
            }
        }

        private void SwitchPlayer()
        {
            //_gameScene.CurrentPlayer = (_gameScene.CurrentPlayer == _gameScene.PlayerWhite) ?
            //               _gameScene.PlayerBlack : _gameScene.PlayerWhite;

            if(_gameScene.CurrentPlayer == _gameScene.PlayerWhite.Color) {
                _gameScene.CurrentPlayer = _gameScene.PlayerBlack.Color;
                _gameScene.Cursor = new Position(2, 3);
            }
            else {
                _gameScene.CurrentPlayer = _gameScene.PlayerWhite.Color;
                _gameScene.Cursor = new Position(5, 4);
            }
        }

        private void MoveCursor(int dRow, int dCol)
        {
            var newPosition = new Position(_gameScene.Cursor.Row + dRow, _gameScene.Cursor.Col + dCol);
            if (_gameScene.Board.IsInsideBoard(newPosition))
                _gameScene.Cursor = newPosition;
        }

        public bool IsGameOver()
        {
            return _isGameOver;
        }
    }
}
