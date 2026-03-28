using Chess.UI.CLI.Panels.BasePanels.Rendering;

namespace Chess.UI.CLI.Panels.BasePanels
{
    public abstract class GraphicsPanelBase<ViewType> : BasePanel<ViewType>
    {
        protected CellRender[,] _buffer;
        protected CellRender[,] _prevBuffer;
        public GraphicsPanelBase(int x, int y, int width, int height) : base(x, y, width, height)
        {
            _buffer = new CellRender[InnerHeight, InnerWidth];
            _prevBuffer = new CellRender[InnerHeight, InnerWidth];

            for (int row = 0; row < InnerHeight; row++)
                for (int col = 0; col < InnerWidth; col++) {
                    _buffer[row, col] = new CellRender();
                    _buffer[row, col].Symbol = ' ';

                    _prevBuffer[row, col] = new CellRender();
                    _prevBuffer[row, col].Symbol = ' ';
                }
        }

        //protected void SetChar(int x, int y, char ch)
        //{
        //    if (x < 0 || x >= InnerWidth || y < 0 || y >= InnerHeight)
        //        return;

        //    _buffer[y, x] = ch;
        //}

        //protected void WriteString(int x, int y, string text)
        //{
        //    for (int i = 0; i < text.Length; i++) {
        //        int col = x + i;
        //        if (col >= Width) break;

        //        _buffer[y, col] = text[i];
        //    }
        //}

        public override void Flush()
        {
            //var row = new char[InnerWidth];
            //for (int r = 0; r < InnerHeight; r++) {
            //    bool changed = false;

            //    for (int c = 0; c < InnerWidth; c++) {
            //        if (_buffer[r, c] != _prevBuffer[r, c]) {
            //            changed = true;
            //            break;
            //        }
            //    }

            //    if (changed) {
            //        // собираем строку
            //        for (int c = 0; c < InnerWidth; c++) {
            //            row[c] = _buffer[r, c].Symbol;
            //            _prevBuffer[r, c].Symbol = _buffer[r, c].Symbol;
            //            _prevBuffer[r, c].fg = _buffer[r, c].fg;
            //            _prevBuffer[r, c].bg = _buffer[r, c].bg;
            //        }

            //        Console.SetCursorPosition(InnerX, InnerY + r);
            //        Console.Write(row);
            //    }
            //}

            var defaultForeground = Console.ForegroundColor;
            var defaultBackground = Console.BackgroundColor;
            for (int row = 0; row < InnerHeight; row++) {
                for (int col = 0; col < InnerWidth; col++) {
                    var current = _buffer[row, col];
                    var prev = _prevBuffer[row, col];

                    if (!current.Equals(prev)) {
                        Console.SetCursorPosition(X + col, Y + row);

                        Console.ForegroundColor = current.fg;
                        Console.BackgroundColor = current.bg;
                        Console.Write(current.Symbol);
                    }
                }
            }
            Console.ForegroundColor = defaultForeground;
            Console.BackgroundColor = defaultBackground;
        }

        protected void ClearBuffer()
        {
            for (int r = 0; r < Height; r++)
                for (int c = 0; c < Width; c++)
                    _buffer[r, c].Symbol = ' ';
        }
    }
}
