using Chess.Application;
using Chess.Domain;

namespace Chess.UI.CLI.Panels
{
    public abstract class BasePanel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int InnerWidth => Width - 2;
        public int InnerHeight => Height - 2;
        public int InnerX => X + 1;
        public int InnerY => Y + 1;

        protected string[] _buffer;
        protected string[] _prevBuffer;

        public BasePanel(int x, int y, int width, int height) //координаты и размер панели
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;

            _buffer = new string[InnerHeight];
            _prevBuffer = new string[InnerHeight];

            for (int i = 0; i < InnerHeight; i++) {
                _buffer[i] = new string(' ', InnerWidth);
                _prevBuffer[i] = new string(' ', InnerWidth);
            }
        }

        public void Render(GamePosition position, GameScene scene)
        {
            DrawBorder();
            BuildBuffer(position, scene);      // заполняем _buffer
            Flush();            // выводим только изменения
        }

        public abstract void BuildBuffer(GamePosition position, GameScene scene); //_buffer
        private void Flush()
        {
            //TODO diff char redraw
            //сравнивать не строки, а символы(char buffer)
            //обновлять только изменённые символы
            for (int row = 0; row < InnerHeight; row++) {
                if (_buffer[row] != _prevBuffer[row]) {
                    int x = InnerX;
                    int y = InnerY + row;

                    if (y >= Console.BufferHeight)
                        continue;

                    Console.SetCursorPosition( Math.Min(x, Console.BufferWidth - 1), y);

                    string line = _buffer[row];

                    int maxWidth = Console.BufferWidth - x;
                    if (maxWidth <= 0) continue;

                    if (line.Length > maxWidth)
                        line = line.Substring(0, maxWidth);

                    Console.Write(line);

                    _prevBuffer[row] = _buffer[row];
                }
            }
        }


        protected void DrawBorder()
        {
            // верх
            SafeWrite(X, Y, "┌" + new string('─', Math.Max(0, Width - 2)) + "┐");

            // низ
            SafeWrite(X, Y + Height - 1, "└" + new string('─', Math.Max(0, Width - 2)) + "┘");

            // боковые
            for (int i = 1; i < Height - 1; i++) {
                SafeWrite(X, Y + i, "│");
                SafeWrite(X + Width - 1, Y + i, "│");
            }
        }

        protected void SafeWrite(int x, int y, string text)
        {
            if (y < 0 || y >= Console.BufferHeight)
                return;

            if (x < 0 || x >= Console.BufferWidth)
                return;

            int maxWidth = Console.BufferWidth - x;
            if (maxWidth <= 0)
                return;

            if (text.Length > maxWidth)
                text = text.Substring(0, maxWidth);

            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }

        public void Clear()
        {
            for (int row = 0; row < InnerHeight; row++) {
                Console.SetCursorPosition(InnerX, InnerY + row);
                Console.Write(new string(' ', InnerWidth));
            }
        }
    }
}
