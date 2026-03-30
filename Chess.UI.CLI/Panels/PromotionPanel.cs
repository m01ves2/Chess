using Chess.Application;
using Chess.Application.ViewModels;
using Chess.UI.CLI.Models;
using Chess.UI.CLI.Panels.BasePanels;

namespace Chess.UI.CLI.Panels
{
    public class PromotionPanel : GraphicsPanelBase<BoardView>
    {
        private const int _cellWidth = 3;
        private const int _cellHeight = 3;
        private const int _promotionBoardWidth = 4;

        private UiPosition promotionCursor = new UiPosition(0, 0);
        private BoardView _promotionView;

        public PromotionPanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }

        public override void BuildBuffer(BoardView view)
        {
            for (int col = 0; col < _promotionBoardWidth; col++) {
                RenderCell(col, view);
            }
        }
        private void RenderCell(int col, BoardView view)
        {
            var cell = view.Cells[0, col];
            bool isCursor = promotionCursor.Col == col && promotionCursor.Row == 0;

            var pieceSymbol = GetPieceSymbol(cell.PieceView.Type);

            BuildCell(col, pieceSymbol, isCursor);
        }
        private static char GetPieceSymbol(PieceViewType type)
        {
            return type switch
            {
                PieceViewType.Rook => '\u265C',
                PieceViewType.Knight => '\u265E',
                PieceViewType.Bishop => '\u265D',
                PieceViewType.Queen => '\u265B',
                _ => '?'
            };
        }

        private void BuildCell(int col, char pieceSymbol, bool isCursor)
        {
            int x = col * _cellWidth;
            int y = 0;
            var fg = ConsoleColor.Black;
            var bg = ConsoleColor.Blue;

            //top
            _buffer[0, x].Symbol = isCursor ? '┌' : ' ';
            _buffer[0, x + 1].Symbol = isCursor ? '─' : ' ';
            _buffer[0, x + 2].Symbol = isCursor ? '┐' : ' ';
            _buffer[0, x].bg = bg;
            _buffer[0, x + 1].bg = bg;
            _buffer[0, x + 2].bg = bg;
            _buffer[0, x].fg = fg;
            _buffer[0, x + 1].fg = fg;
            _buffer[0, x + 2].fg = fg;
            //middle
            _buffer[1, x].Symbol = isCursor ? '│' : ' ';
            _buffer[1, x + 1].Symbol = pieceSymbol;
            _buffer[1, x + 2].Symbol = isCursor ? '│' : ' ';
            _buffer[1, x].bg = bg;
            _buffer[1, x + 1].bg = bg;
            _buffer[1, x + 2].bg = bg;
            _buffer[1, x].fg = fg;
            _buffer[1, x + 1].fg = fg;
            _buffer[1, x + 2].fg = fg;
            //bottom
            _buffer[2, x].Symbol = isCursor ? '└' : ' ';
            _buffer[2, x + 1].Symbol = isCursor ? '─' : ' ';
            _buffer[2, x + 2].Symbol = isCursor ? '┘' : ' ';
            _buffer[2, x].bg = bg;
            _buffer[2, x + 1].bg = bg;
            _buffer[2, x + 2].bg = bg;
            _buffer[2, x].fg = fg;
            _buffer[2, x + 1].fg = fg;
            _buffer[2, x + 2].fg = fg;
        }

        public void MoveLeft()
        {
            if (promotionCursor.Col > 0)
                promotionCursor.Col--;
        }

        public void MoveRight()
        {
            if (promotionCursor.Col < _promotionBoardWidth - 1)
                promotionCursor.Col++;
        }

        public PieceViewType Select(BoardView promotionView)
        {
            return _promotionView.Cells[promotionCursor.Row, promotionCursor.Col].PieceView.Type;
        }
    }
}
