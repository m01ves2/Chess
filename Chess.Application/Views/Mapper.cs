using Chess.Domain;
using Chess.Domain.Moves;
using Chess.Domain.Pieces;

namespace Chess.Application.Views
{
    static class Mapper
    {
        public static BoardView GetBoardView(Board board, Position? selected, List<Position> highlights, PieceColor currentPlayerColor)
        {
            var cells = new CellView[Board.BoardSize, Board.BoardSize];
            
            var highlightsHash = new HashSet<Position>(highlights);

            for (int row = 0; row < Board.BoardSize; row++) {
                for (int col = 0; col < Board.BoardSize; col++) {
                    var square = board.Squares[row, col];
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

                    if (highlights != null)
                        if (highlights.Contains(new Position(row, col)))
                            cell.IsHighlighted = true;

                    cells[row, col] = cell;
                }
            }

            if (selected != null)
                cells[selected.Value.Row, selected.Value.Col].IsSelected = true;

            return new BoardView { Cells = cells, currentPlayer = currentPlayerColor == PieceColor.White ? PieceViewColor.White : PieceViewColor.Black };
        }

        private static PieceViewType MapPieceType(Piece piece) => piece switch
        {
            Bishop => PieceViewType.Bishop,
            Knight => PieceViewType.Knight,
            Rook => PieceViewType.Rook,
            Queen => PieceViewType.Queen,
            King => PieceViewType.King,
            Pawn => PieceViewType.Pawn,
            _ => throw new Exception("Unknown piece")
        };

        public static MoveHistoryView GetHistoryView(IReadOnlyList<Move> _moveHistory)
        {
            var historyViewModel = new MoveHistoryView();
            for (int i = 0; i < _moveHistory.Count; i++) {
                var mh = _moveHistory[i];
                string mitem = $"#{i + 1}.{NotationMapper.PositionToString(mh.From)} - {NotationMapper.PositionToString(mh.To)}";

                if (mh is NormalMove nm && nm.CapturedPiece != null) {
                    mitem += PieceToChar(nm.CapturedPiece);
                }
                else if (mh is EnPassantMove em && em.CapturedPiece != null) {
                    mitem += PieceToChar(em.CapturedPiece);
                }
                historyViewModel.Moves.Add(mitem);
            }
            return historyViewModel;
        }
        private static char PieceToChar(Piece piece) => piece switch
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

        public static InfoView GetInfoView(PieceColor currentPlayer, bool isCheck,  bool isPromotionPending)
        {
            var infoView = new InfoView()
            {
                CurrentPlayer = currentPlayer == PieceColor.White ? PieceViewColor.White : PieceViewColor.Black,
                IsCheck = isCheck,
                IsPromotionPending = isPromotionPending,
            };
            return infoView;
        }

        public static CapturedView GetCapturedView(List<Piece> whiteCaptured, List<Piece> blackCaptured)
        {
            var capturedView = new CapturedView();
            foreach (var captured in whiteCaptured) {
                capturedView.WhiteCaptured.Add(MapPieceType(captured));
            }
            foreach (var captured in blackCaptured) {
                capturedView.BlackCaptured.Add(MapPieceType(captured));
            }
            return capturedView;
        }

        public static Piece PieceViewTypeToPiece(PieceViewType type, PieceColor color) => type switch
        {
            PieceViewType.Queen => new Queen(color),
            PieceViewType.Rook => new Rook(color),
            PieceViewType.Knight => new Knight(color),
            PieceViewType.Bishop => new Bishop(color),
            _ => new Queen(color),
        };
    }
}
