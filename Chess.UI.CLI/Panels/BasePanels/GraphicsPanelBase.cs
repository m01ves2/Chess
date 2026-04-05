using Chess.UI.CLI.Panels.BasePanels.Rendering;

namespace Chess.UI.CLI.Panels.BasePanels
{
    public abstract class GraphicsPanelBase<ViewType> : BasePanel<ViewType>
    {
        protected CellRender[,] _buffer;
        public GraphicsPanelBase(int x, int y, int width, int height) : base(x, y, width, height)
        {
            _buffer = new CellRender[InnerHeight, InnerWidth];

            for (int row = 0; row < InnerHeight; row++)
                for (int col = 0; col < InnerWidth; col++) {
                    _buffer[row, col] = new CellRender();
                    _buffer[row, col].Symbol = ' ';
                }
        }

        public override void CopyToScreen(CellRender[,] _screenBuffer)
        {
            for (int row = 0; row < InnerHeight; row++) {
                for (int col = 0; col < InnerWidth; col++) {
                    var current = _buffer[row, col];

                    if (_screenBuffer[Y + 1 + row, X + 1 + col] != current) {
                        _screenBuffer[Y + 1 + row, X + 1 + col].Symbol = current.Symbol;
                        _screenBuffer[Y + 1 + row, X + 1 + col].fg = current.fg;
                        _screenBuffer[Y + 1 + row, X + 1 + col].bg = current.bg;
                    }
                }
            }
        }

        public override void ClearBuffer()
        {
            for (int r = 0; r < InnerHeight; r++)
                for (int c = 0; c < InnerWidth; c++) {
                    _buffer[r, c].Symbol = ' ';
                    _buffer[r, c].fg = Console.ForegroundColor;
                    _buffer[r, c].bg = Console.BackgroundColor;
                }
        }
    }
}
