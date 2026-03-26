using Chess.Application.ViewModels;
using Chess.Domain;
using Chess.Domain.Moves;
using Chess.Engine;

namespace Chess.Application
{
    public class GameController
    {
        private readonly GameScene _gameScene;
        private readonly GameEngine _gameEngine;
        private GamePosition _gamePosition;

        public GamePosition Position => _gamePosition;
        public GameScene Scene => _gameScene;

        private List<Move> _moveHistory { get; set; } = new List<Move>();
        private Stack<GameSnapshot> _snapshotHistory = new Stack<GameSnapshot>();

        private bool _isGameOver = false;

        public GameController()
        {
            _gameScene = new GameScene();
            _gamePosition = new GamePosition(new Board(), new GameState(), PieceColor.White);
            _gameEngine = new GameEngine();

            // добавляем стартовый snapshot в стек
            _snapshotHistory.Push(CreateSnapshot());
        }

        // Обработка действия игрока
        //public void ProcessAction(PlayerAction action)
        //{
        //    if (action == null) return;

        //    switch (action.Type) {
        //        case PlayerActionType.MoveUp:
        //            MoveCursor(-1, 0);
        //            break;
        //        case PlayerActionType.MoveDown:
        //            MoveCursor(1, 0);
        //            break;
        //        case PlayerActionType.MoveLeft:
        //            MoveCursor(0, -1);
        //            break;
        //        case PlayerActionType.MoveRight:
        //            MoveCursor(0, 1);
        //            break;

        //        case PlayerActionType.Select:
        //            Select(_gameScene.Cursor);
        //            break;
                

        //        case PlayerActionType.Undo:
        //            UndoMove();
        //            break;
        //        case PlayerActionType.NewGame:
        //            // TODO: сброс игры
        //            break;
        //        case PlayerActionType.Quit:
        //            _isGameOver = true;
        //            break; // завершаем игру

        //        default:
        //            break;
        //    }
        //}

        public void Select(Position position)
        {
            // Сброс подсветки, если новая клетка выбрана
            var selectedSquare = _gamePosition.Board.GetSquare(position);
            _gameScene.HighlightedPositions.Clear();

            if (_gameScene.SelectedPosition == position) {
                _gameScene.ClearSelection();
                return;
            }

            if (selectedSquare.Piece?.Color == _gamePosition.CurrentPlayer) {
                var result = SelectPiece(position);
                _gameScene.SetSelection(result.SelectedPosition!.Value);
                _gameScene.SetHighlights(result.AvailableMoves);
                return;
            }

            if (_gameScene.SelectedPosition != null) {
                TryMakeMove(_gameScene.SelectedPosition.Value, position);
                _gameScene.ClearSelection();
            }
        }

        //private bool IsSameCell(Position position)
        //{
        //    return _gameScene.SelectedPosition == position;
        //}
        //private void DeselectCell()
        //{
        //    _gameScene.SelectedPosition = null;
        //}

        //private bool IsOwnPiece(Position position)
        //{
        //    var selectedSquare = _gamePosition.Board.GetSquare(position);
        //    return selectedSquare.Piece?.Color == _gamePosition.CurrentPlayer;
        //}

        //private void SelectPiece(Position position)
        //{
        //    _gameScene.SelectedPosition = position;
        //    var moves = _gameEngine.GetLegalMoves(_gamePosition, position).ToList();
        //    moves.ForEach(m => _gameScene.HighlightedPositions.Add(m.To));
        //    return;
        //}
        private SelectionResult SelectPiece(Position position)
        {
            var moves = _gameEngine.GetLegalMoves(_gamePosition, position).Select(m => m.To).ToList();
            return new SelectionResult(position, moves);
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
            //if (move is PromotionMove pm && pm.IsChoicePending) {
            //    // вызвать UI, чтобы игрок выбрал фигуру
            //    var chosen = _gameScene.AskPromotionChoice(pm.Piece.Color, pm.To); // метод возвращает Type
            //    pm.PromotedPieceType = chosen;
            //    pm.IsChoicePending = false;
            //}

            _snapshotHistory.Push(CreateSnapshot()); // snapshot ДО хода
            _gameEngine.MakeMove(_gamePosition, move);      // применяем ход один раз
            _moveHistory.Add(move);
            SwitchPlayer();
            UpdateGameScene();
        }

        public void UndoMove()
        {
            _gameScene.ClearSelection();
            _gameScene.HighlightedPositions.Clear();
            if (_snapshotHistory.Count > 1 && _moveHistory.Count > 0) {
                var snapshot = _snapshotHistory.Pop();
                _moveHistory.Remove(_moveHistory.Last());

                RestoreSnapshot(snapshot);
                UpdateGameScene();
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

        public void MoveCursor(int dRow, int dCol)
        {
            var newPosition = new Position(_gameScene.Cursor.Row + dRow, _gameScene.Cursor.Col + dCol);
            if (_gamePosition.Board.IsInsideBoard(newPosition))
                _gameScene.Cursor = newPosition;
        }

        public bool IsGameOver()
        {
            return _isGameOver;
        }

        public void UpdateGameScene()
        {
            _gameScene.UpdateMoveHistory(_moveHistory);
            _gameScene.WhiteKingInCheck = _gameEngine.IsKingInCheck(_gamePosition, PieceColor.White);
            _gameScene.BlackKingInCheck = _gameEngine.IsKingInCheck(_gamePosition, PieceColor.Black);
        }

        public BoardViewModel GetBoardView()
        {
            //var vm = new BoardViewModel();

            //for (int row = 0; row < 8; row++) {
            //    for (int col = 0; col < 8; col++) {
            //        var square = _gamePosition.Board.GetSquare(new Position(row, col));

            //        vm.Cells[row, col] = square.Piece == null ? ' ' : MapPieceToChar(square.Piece);
            //    }
            //}

            //vm.Cursor = _gameScene.Cursor;
            //vm.Highlights = _gameScene.HighlightedPositions.ToList();

            //return vm;
            throw new NotImplementedException();
        }
        public MoveHistoryViewModel GetHistoryView()
        {
            var historyViewModel = new MoveHistoryViewModel();
            for(int  i = 0; i < _moveHistory.Count; i++) {
                var mh = _moveHistory[i];
                string mitem = $"#{i + 1}.{NotationMapper.PositionToString(mh.From)} - {NotationMapper.PositionToString(mh.To)}";
                
                if (mh is NormalMove nm && nm.CapturedPiece != null) {
                    mitem += nm.CapturedPiece.ToString();
                }
                else if (mh is EnPassantMove em && em.CapturedPiece != null) {
                    mitem += em.CapturedPiece.ToString();
                }
                historyViewModel.MoveHistory.Add(mitem);
            }
            return historyViewModel;
        }
        public InfoViewModel GetInfoView()
        {
            var infoViewModel = new InfoViewModel();

            infoViewModel.Info.Add(_gamePosition.CurrentPlayer.ToString() + " turns.");

            if (_gameScene.WhiteKingInCheck)
                infoViewModel.Info.Add("White King in Check!");

            if(_gameScene.BlackKingInCheck)
                infoViewModel.Info.Add("Black King in Check!");

            return infoViewModel;
        }
    }
}
