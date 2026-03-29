using Chess.Application.Models;
using Chess.Application.ViewModels;
using Chess.Domain;
using Chess.Domain.Pieces;
using Chess.UI.CLI.Panels.BasePanels;
using Chess.UI.CLI.Screens;
using System.Data;

namespace Chess.UI.CLI.Panels
{
    public class BoardPanel : GraphicsPanelBase<BoardView>
    {
        private const int _cellWidth = 3;
        private const int _cellHeight = 3;
        private const int _boardSize = 8;

        public BoardPanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }

        public override void BuildBuffer(BoardView view)
        {
            RenderFiles();
            BuildBoard(view);
            BuildRanks();
        }
        private void RenderFiles()
        {
            //Верхняя нумерация файлов(a - h)
            int d = 1;
            int XOffset = 0;
            int YOffset = 1;
            for (char f = 'a'; f <= 'h'; f++) {
                _buffer[YOffset, _cellWidth * d + XOffset].Symbol = f;
                _buffer[YOffset, _cellWidth * d + XOffset].fg = ConsoleColor.Yellow;
                _buffer[YOffset, _cellWidth * d + XOffset].bg = ConsoleColor.Black;
                d++;
            }
        }
        private void BuildRanks()
        {
            //Левый столбец с рангами(1 - 8)
            int YOffset = 3;
            int XOffset = 1;
            for (int r = 0; r < _boardSize; r++) {
                _buffer[r * _cellHeight + YOffset, XOffset].Symbol = char.Parse((8 - r).ToString());
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
            bool isCursor = cell.IsCursor;
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
            int XOffset = 2;
            int YOffset = 2;

            int x = XOffset + col * _cellWidth;
            int y = YOffset + row * _cellHeight;

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
        private (int row, int col) Rotate180(int row, int col)
        {
            return (7 - row, 7 - col);
        }
    }
}