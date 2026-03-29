using Chess.Application.Models;
using Chess.Application.ViewModels;
using Chess.Domain;
using Chess.Domain.Moves;
using Chess.Domain.Pieces;
using Chess.Engine;

namespace Chess.Application
{
    public class GameController
    {
        private readonly GameScene _gameScene;
        private readonly GameEngine _gameEngine;
        private GamePosition _gamePosition;

        private PromotionMove? _pendingPromotion;
        public bool IsPromotionPending => _pendingPromotion != null;

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

            if (move is PromotionMove promotionMove) {
                _pendingPromotion = promotionMove; // НЕ делаем ход
                return;
            }
            DoMove(move);
        }

        public void CompletePromotion(PieceViewType type)
        {
            if (_pendingPromotion == null)
                return;
            var piece = Convert(type);
            _pendingPromotion.SetPromotionPiece(piece);
            DoMove(_pendingPromotion);
            _pendingPromotion = null;
        }
        private Piece Convert(PieceViewType type) => type switch
        {
            PieceViewType.Queen => new Queen(_gamePosition.CurrentPlayer),
            PieceViewType.Rook => new Rook(_gamePosition.CurrentPlayer),
            PieceViewType.Knight => new Knight(_gamePosition.CurrentPlayer),
            PieceViewType.Bishop => new Bishop(_gamePosition.CurrentPlayer),
            _ => new Queen(_gamePosition.CurrentPlayer),
        };

        //public void Handle(PlayerAction action)
        //{
        //    // если ждём promotion
        //    if (_pendingPromotion != null) {
        //        if (action.Type == PlayerActionType.Promotion) {
        //            CompletePromotion(action.PromotionPiece.Value);
        //        }
        //        else if (action.Type == PlayerActionType.Cancel) {
        //            CancelPromotion();
        //        }

        //        return; // всё остальное игнорируем
        //    }

        //    // обычная логика
        //    HandleNormal(action);
        //}

        private void CancelPromotion()
        {
            _pendingPromotion = null;

            // возможно:
            // сбросить выбор клетки
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

        public BoardView GetBoardView()
        {
            var cells = new CellView[Board.BoardSize, Board.BoardSize];
            var highlights = new HashSet<Position>(_gameScene.HighlightedPositions);

            for (int row = 0; row < Board.BoardSize; row++) {
                for (int col = 0; col < Board.BoardSize; col++) {
                    var square = _gamePosition.Board.Squares[row, col];
                    var cell = new CellView();

                    if (!square.IsEmpty()) {
                        var piece = square.Piece;

                        cell.PieceView = new PieceView
                        {
                            Color = piece.Color == PieceColor.White
                                ? PieceViewColor.White
                                : PieceViewColor.Black,
                            Type = MapPieceType(piece)
                        };
                    }

                    if (highlights.Contains(new Position(row, col)))
                        cell.IsHighlighted = true;

                    cells[row, col] = cell;
                }
            }

            var cursor = _gameScene.Cursor;
            cells[cursor.Row, cursor.Col].IsCursor = true;

            if (_gameScene.SelectedPosition is Position selected)
                cells[selected.Row, selected.Col].IsSelected = true;

            return new BoardView { Cells = cells };
        }

        private PieceViewType MapPieceType(Piece piece) => piece switch
        {
            Bishop => PieceViewType.Bishop,
            Knight => PieceViewType.Knight,
            Rook => PieceViewType.Rook,
            Queen => PieceViewType.Queen,
            King => PieceViewType.King,
            Pawn => PieceViewType.Pawn,
            _ => throw new Exception("Unknown piece")
        };

        public MoveHistoryView GetHistoryView()
        {
            var historyViewModel = new MoveHistoryView();
            for (int i = 0; i < _moveHistory.Count; i++) {
                var mh = _moveHistory[i];
                string mitem = $"#{i + 1}.{NotationMapper.PositionToString(mh.From)} - {NotationMapper.PositionToString(mh.To)}";

                if (mh is NormalMove nm && nm.CapturedPiece != null) {
                    mitem += GetSymbol(nm.CapturedPiece);
                }
                else if (mh is EnPassantMove em && em.CapturedPiece != null) {
                    mitem += GetSymbol(em.CapturedPiece);
                }
                historyViewModel.Moves.Add(mitem);
            }
            return historyViewModel;
        }

        private static char GetSymbol(Piece piece) => piece switch
        {
            Rook r when r.Color == PieceColor.White => '\u2656',
            Rook r => '\u265C',
            Knight n when n.Color == PieceColor.White => '\u2658',
            Knight n => '\u265E',
            Bishop b when b.Color == PieceColor.White => '\u2657',
            Bishop b => '\u265D',
            Queen q when q.Color == PieceColor.White => '\u2655',
            Queen q => '\u265B',
            King k when k.Color == PieceColor.White => '\u2654',
            King k => '\u265A',
            Pawn p when p.Color == PieceColor.White => '\u2659',
            Pawn p => '\u265F',
            _ => '?'
        };

        public InfoView GetInfoView()
        {
            var infoView = new InfoView()
            {
                CurrentPlayer = (_gamePosition.CurrentPlayer == PieceColor.White ? PieceViewColor.White : PieceViewColor.Black),
                IsCheck = _gameScene.WhiteKingInCheck,
                IsCheckmate = false, //TODO
                IsPromoted = IsPromotionPending,
            };
            return infoView;
        }

        public CapturedView GetCapturedView()
        {
            var capturedView = new CapturedView();
            foreach (var captured in _gamePosition.Board.WhiteCaptured) {
                capturedView.WhiteCaptured.Add(MapPieceType(captured));
            }
            foreach (var captured in _gamePosition.Board.BlackCaptured) {
                capturedView.BlackCaptured.Add(MapPieceType(captured));
            }
            return capturedView;
        }
    }
}
