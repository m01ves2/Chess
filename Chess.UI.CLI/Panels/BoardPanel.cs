using Chess.Application.Views;
using Chess.UI.CLI.Models;
using Chess.UI.CLI.Panels.BasePanels;
using Chess.UI.CLI.Views;

namespace Chess.UI.CLI.Panels
{
    public class BoardPanel : GraphicsPanelBase<BoardDisplayView>
    {
        private const int _cellWidth = 3;
        private const int _cellHeight = 3;

        private const int _boardSize = 8;
        
        private UiPosition cursor; //[0...7, 0...7]
        private bool _flipped = false;
        public UiPosition boardCursor => _flipped ? Rotate180(cursor) : cursor;

        public BoardPanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
            cursor = new UiPosition(_boardSize/2, _boardSize/2);
        }

        public override void BuildBuffer(BoardDisplayView view)
        {
            if (view.IsBoardFlipped)
                RotateView180(view);

            RenderFiles(view.IsBoardFlipped);
            BuildBoard(view.BoardView);
            BuildRanks(view.IsBoardFlipped);
        }
        private void RenderFiles(bool flipped)
        {
            //Верхняя нумерация файлов(a - h)
            int d = 1;
            int XOffset = 0;
            int YOffset = 1;
            for (int f = 0; f < _boardSize; f++) {
                if (flipped) {
                    _buffer[YOffset, _cellWidth * d + XOffset].Symbol = (char)('h' - f);
                }
                else {
                    _buffer[YOffset, _cellWidth * d + XOffset].Symbol = (char)('a' + f);
                }
                _buffer[YOffset, _cellWidth * d + XOffset].fg = ConsoleColor.Yellow;
                _buffer[YOffset, _cellWidth * d + XOffset].bg = ConsoleColor.Black;
                d++;
            }
        }
        private void BuildRanks(bool flipped)
        {
            _flipped = flipped;
            //Левый столбец с рангами(1 - 8)
            int YOffset = 3;
            int XOffset = 1;
            for (int r = 0; r < _boardSize; r++) {
                if (flipped) {
                    _buffer[r * _cellHeight + YOffset, XOffset].Symbol = char.Parse((8 - r).ToString());
                }
                else {
                    _buffer[r * _cellHeight + YOffset, XOffset].Symbol = char.Parse((1 + r).ToString());
                }
                _buffer[r * _cellHeight + YOffset, XOffset].fg = ConsoleColor.Yellow;
                _buffer[r * _cellHeight + YOffset, XOffset].bg = ConsoleColor.Black;
            }
        }
        private void BuildBoard(BoardView view)
        {
            for (int row = 0; row < _boardSize; row++) {
                for (int col = 0; col < _boardSize; col++) {
                    RenderCell(row, col, view);
                }
            }
        }
        private void RenderCell(int row, int col, BoardView view)
        {
            var cell = view.Cells[row, col];
            //bool isCursor = cell.IsCursor;
            bool isCursor = (cursor.Row == row && cursor.Col == col);
            bool isSelected = cell.IsSelected;
            bool isHighlighted = cell.IsHighlighted;

            ConsoleColor bg = GetBackgroundColor(row, col, isSelected, isHighlighted);
            ConsoleColor fg = GetPieceColor(cell) ?? ConsoleColor.Red;

            var pieceSymbol = GetPieceSymbol(cell.PieceView );
            
            BuildCell(row, col, pieceSymbol, bg, fg, isCursor);
        }
        private ConsoleColor GetBackgroundColor(int row, int col, bool selected, bool highlighted)
        {
            if (selected)
                return ConsoleColor.DarkGreen;
            if (highlighted)
                return ConsoleColor.DarkYellow;

            return (row + col) % 2 == 0 ? ConsoleColor.Gray : ConsoleColor.DarkGray;
        }
        private ConsoleColor? GetPieceColor(CellView cell)
        {
            if (cell.PieceView == null)
                return null;
            else if (cell.PieceView.Color == PieceViewColor.Black)
                return ConsoleColor.Black;
            else
                return ConsoleColor.Yellow;
        }
        private char GetPieceSymbol(PieceView? pieceView)
        {
            return pieceView == null ? ' ' : GetSymbol(pieceView.Type, pieceView.Color);
        }
        private void BuildCell(int row, int col, char pieceSymbol, ConsoleColor bg, ConsoleColor fg, bool isCursor)
        {
            int _boardXOffset = 2;
            int _boardYOffset = 2;

            int x = _boardXOffset + col * _cellWidth;
            int y = _boardYOffset + row * _cellHeight;

            //top
            _buffer[y, x    ].Symbol = isCursor ? '┌' : ' ';
            _buffer[y, x + 1].Symbol = isCursor ? '─' : ' ';
            _buffer[y, x + 2].Symbol = isCursor ? '┐' : ' ';
            _buffer[y, x    ].bg = bg;
            _buffer[y, x + 1].bg = bg;
            _buffer[y, x + 2].bg = bg;
            _buffer[y, x    ].fg = fg;
            _buffer[y, x + 1].fg = fg;
            _buffer[y, x + 2].fg = fg;
            //middle
            _buffer[y + 1, x    ].Symbol = isCursor ? '│' : ' ';
            _buffer[y + 1, x + 1].Symbol = pieceSymbol;
            _buffer[y + 1, x + 2].Symbol = isCursor ? '│' : ' ';
            _buffer[y + 1, x    ].bg = bg;
            _buffer[y + 1, x + 1].bg = bg;
            _buffer[y + 1, x + 2].bg = bg;
            _buffer[y + 1,     x].fg = fg;
            _buffer[y + 1, x + 1].fg = fg;
            _buffer[y + 1, x + 2].fg = fg;
            //bottom
            _buffer[y + 2, x    ].Symbol = isCursor ? '└' : ' ';
            _buffer[y + 2, x + 1].Symbol = isCursor ? '─' : ' ';
            _buffer[y + 2, x + 2].Symbol = isCursor ? '┘' : ' ';
            _buffer[y + 2, x    ].bg = bg;
            _buffer[y + 2, x + 1].bg = bg;
            _buffer[y + 2, x + 2].bg = bg;
            _buffer[y + 2, x    ].fg = fg;
            _buffer[y + 2, x + 1].fg = fg;
            _buffer[y + 2, x + 2].fg = fg;
        }

        private void RotateView180(BoardDisplayView view)
        {
            var board = view.BoardView.Cells;
            int height = board.GetLength(0);
            int width = board.GetLength(1);

            for (int y = 0; y < height / 2; y++) {
                for (int x = 0; x < width; x++) {
                    int oppY = height - 1 - y;
                    int oppX = width - 1 - x;

                    var tmp = board[y, x];
                    board[y, x] = board[oppY, oppX];
                    board[oppY, oppX] = tmp;
                }
            }
        }
        public UiPosition Rotate180(UiPosition uiPosition)
        {
            return new UiPosition(7 - uiPosition.Row, 7 - uiPosition.Col);
        }

        public void MoveUp()
        {
            if (cursor.Row > 0)
                cursor.Row--;
        }

        public void MoveDown()
        {
            if(cursor.Row < _boardSize - 1)
                cursor.Row++;
        }

        public void MoveLeft()
        {
            if(cursor.Col > 0) {
                cursor.Col--;
            }
        }
        public void MoveRight()
        {
            if(cursor.Col < _boardSize - 1)
                cursor.Col++;
        }
    }
}