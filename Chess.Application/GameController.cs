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
        public Guid Id { get; } = Guid.NewGuid();

        private readonly GameScene _gameScene;
        private readonly GameEngine _gameEngine;
        private GamePosition _gamePosition;

        private PromotionMove? _pendingPromotionMove;
        private Move? _pendingMove;
        public bool IsPromotionPending => _pendingPromotionMove != null;

        public GamePosition GamePosition => _gamePosition;
        public GameScene Scene => _gameScene;

        private List<Move> _moveHistory { get; set; } = new List<Move>();
        private Stack<GameSnapshot> _snapshotHistory = new Stack<GameSnapshot>();


        private bool _isGameOver = false;
        public bool IsGameOver => _isGameOver;
        public PieceColor Winner { get; private set; }


        public GameController()
        {
            //Console.WriteLine($"Controller ID: {Id}");
            System.Diagnostics.Debug.WriteLine($"Controller ID: {Id}");

            _gameScene = new GameScene();
            _gamePosition = new GamePosition(new Board(), new GameState(), PieceColor.White);
            _gameEngine = new GameEngine();

            // добавляем стартовый snapshot в стек
            _snapshotHistory.Push(CreateSnapshot());
        }

        //public void Select(int row, int col)
        //{

        //    System.Diagnostics.Debug.WriteLine($"Select: Controller ID: {Id}");

        //    Position position = new Position(row, col);
        //    // Сброс подсветки, если новая клетка выбрана
        //    var selectedSquare = _gamePosition.Board.GetSquare(position);
        //    _gameScene.HighlightedPositions.Clear();

        //    if (_gameScene.SelectedPosition == position) {
        //        _gameScene.ClearSelection();
        //        return;
        //    }

        //    if (selectedSquare.Piece?.Color == _gamePosition.CurrentPlayer) {
        //        var result = SelectPiece(position);
        //        _gameScene.SetSelection(result.SelectedPosition!.Value);
        //        _gameScene.SetHighlights(result.AvailableMoves);
        //        return;
        //    }

        //    if (_gameScene.SelectedPosition != null) {
        //        TryMakeMove(_gameScene.SelectedPosition.Value, position);
        //        _gameScene.ClearSelection();
        //    }
        //}

        //private SelectionResult SelectPiece(Position position) //TODO убрать SelectionResult! просто отдавать moves
        //{
        //    var moves = _gameEngine.GetLegalMoves(_gamePosition, position).Select(m => m.To).ToList();
        //    return new SelectionResult(position, moves);
        //}


        public void TryMakeMove(Position from, Position to)
        {
            System.Diagnostics.Debug.WriteLine($"TryMakeMove: Controller ID: {Id}");

            var moves = _gameEngine.GetLegalMoves(_gamePosition, from);
            var move = moves.FirstOrDefault(m => m.To == to);

            if (move == null)
                return;

            if (move is PromotionMove promotionMove) {
                _pendingPromotionMove = promotionMove; // НЕ делаем ход
                return;
            }
            //DoMove(move);

            _pendingMove = move;
        }

        public Move? TryGetPendingMove()
        {
            System.Diagnostics.Debug.WriteLine($"TryGetPendingMove: Controller ID: {Id}");
            return _pendingMove;
        }

        public void CompletePromotion(PieceViewType type)
        {
            if (_pendingPromotionMove == null)
                return;
            var piece = Convert(type);
            _pendingPromotionMove.SetPromotionPiece(piece);

            //DoMove(_pendingPromotion);
            _pendingMove = _pendingPromotionMove;

            //_pendingPromotionMove = null; //TODO
        }
        private Piece Convert(PieceViewType type) => type switch
        {
            PieceViewType.Queen => new Queen(_gamePosition.CurrentPlayer),
            PieceViewType.Rook => new Rook(_gamePosition.CurrentPlayer),
            PieceViewType.Knight => new Knight(_gamePosition.CurrentPlayer),
            PieceViewType.Bishop => new Bishop(_gamePosition.CurrentPlayer),
            _ => new Queen(_gamePosition.CurrentPlayer),
        };

        public void CancelPromotion()
        {
            _pendingPromotionMove = null;
            _gameScene.ClearSelection();
        }

        //public Move? GetPendingMove()
        //{
        //     return _pendingMove;
        //}

        public void DoMove(Move move)
        {
            if (move == null) return;

            _snapshotHistory.Push(CreateSnapshot()); // snapshot ДО хода
            _gameEngine.MakeMove(_gamePosition, move);      // применяем ход один раз
            _moveHistory.Add(move);
            //_pendingMove = null;
            SwitchPlayer();
            UpdateGameScene();

            if (_gameEngine.IsCheckmate(_gamePosition, _gamePosition.CurrentPlayer )) {
                //_gameState = GameStatus.GameOver;
                _isGameOver = true;
                Winner = _gamePosition.CurrentPlayer == PieceColor.White ? PieceColor.Black : PieceColor.White;
            }
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
            _gamePosition.SwitchTurn();
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

            if (_gameScene.SelectedPosition is Position selected)
                cells[selected.Row, selected.Col].IsSelected = true;
            
            var currentPlayer = _gamePosition.CurrentPlayer;

            return new BoardView { Cells = cells, currentPlayer = currentPlayer == PieceColor.White ? PieceViewColor.White : PieceViewColor.Black };
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
                IsCheck = _gameScene.WhiteKingInCheck || _gameScene.BlackKingInCheck,
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

        public IEnumerable<Move>  GetAllLegalMoves()
        {
            return _gameEngine.GetAllLegalMoves(_gamePosition, _gamePosition.CurrentPlayer);
        }
    }
}
