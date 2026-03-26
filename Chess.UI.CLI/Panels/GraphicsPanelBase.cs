using Chess.Application;
using Chess.Domain;

namespace Chess.UI.CLI.Panels
{
    public abstract class GraphicsPanelBase : BasePanel
    {
        protected char[,] _buffer;
        protected char[,] _prevBuffer;
        public GraphicsPanelBase(int x, int y, int width, int height) : base(x, y, width, height)
        {
            _buffer = new char[InnerHeight, InnerWidth];
            _prevBuffer = new char[InnerHeight, InnerWidth];

            for (int i = 0; i < InnerHeight; i++)
                for (int j = 0; j < InnerWidth; j++) {
                    _buffer[i, j] = ' ';
                    _prevBuffer[i, j] = ' ';
                }
        }

        protected void SetChar(int x, int y, char ch)
        {
            if (x < 0 || x >= InnerWidth || y < 0 || y >= InnerHeight)
                return;

            _buffer[y, x] = ch;
        }

        protected void WriteString(int x, int y, string text)
        {
            for (int i = 0; i < text.Length; i++) {
                int col = x + i;
                if (col >= Width) break;

                _buffer[y, col] = text[i];
            }
        }

        public override void Flush()
        {
            var row = new char[InnerWidth];
            for (int r = 0; r < InnerHeight; r++) {
                bool changed = false;

                for (int c = 0; c < InnerWidth; c++) {
                    if (_buffer[r, c] != _prevBuffer[r, c]) {
                        changed = true;
                        break;
                    }
                }

                if (changed) {
                    // собираем строку
                    for (int c = 0; c < InnerWidth; c++) {
                        row[c] = _buffer[r, c];
                        _prevBuffer[r, c] = _buffer[r, c];
                    }

                    Console.SetCursorPosition(InnerX, InnerY + r);
                    Console.Write(row);
                }
            }
        }

        protected void ClearBuffer()
        {
            for (int r = 0; r < Height; r++)
                for (int c = 0; c < Width; c++)
                    _buffer[r, c] = ' ';
        }
    }
}
