using Chess.Application;
using Chess.Domain;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

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

        public BasePanel(int x, int y, int width, int height) //координаты и размер панели
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public void Render()
        {
            DrawBorder();
            BuildBuffer();      // заполняем _buffer
            Flush();            // выводим только изменения
        }

        public abstract void BuildBuffer(); //_buffer
        public abstract void Flush();
        //{
            ////TODO diff char redraw
            ////сравнивать не строки, а символы(char buffer)
            ////обновлять только изменённые символы
            //for (int row = 0; row < InnerHeight; row++) {
            //    for (int col = 0; col < InnerWidth; col++) {
            //        if (_buffer[row, col] != _prevBuffer[row, col]) {
            //            _prevBuffer[row, col] = _buffer[row, col];
            //            int x = InnerX + col;
            //            int y = InnerY + row;

            //            Console.SetCursorPosition(Math.Min(x, Console.BufferWidth - 1), Math.Min(y, Console.BufferHeight - 1));
            //            Console.Write(_buffer[row, col]);

            //            //int x = InnerX;
            //            //int y = InnerY + row;

            //            //if (y >= Console.BufferHeight)
            //            //    continue;

            //            //Console.SetCursorPosition( Math.Min(x, Console.BufferWidth - 1), y);

            //            //string line = _buffer[row];

            //            //int maxWidth = Console.BufferWidth - x;
            //            //if (maxWidth <= 0) continue;

            //            //if (line.Length > maxWidth)
            //            //    line = line.Substring(0, maxWidth);

            //            //Console.Write(line);

            //            //_prevBuffer[row] = _buffer[row];
            //        }
            //    }
            //}
        //}


        protected void DrawBorder()
        {
            // верх
            WriteAt(X, Y, "┌" + new string('─', Math.Max(0, Width - 2)) + "┐");

            // низ
            WriteAt(X, Y + Height - 1, "└" + new string('─', Math.Max(0, Width - 2)) + "┘");

            // боковые
            for (int i = 1; i < Height - 1; i++) {
                WriteAt(X, Y + i, "│");
                WriteAt(X + Width - 1, Y + i, "│");
            }
        }

        protected void WriteAt(int x, int y, string text)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }

        protected void SafeWrite(int localX, int localY, string text)
        {
            //if (y < 0 || y >= Console.BufferHeight)
            //    return;

            //if (x < 0 || x >= Console.BufferWidth)
            //    return;

            //int maxWidth = Console.BufferWidth - x;
            //if (maxWidth <= 0)
            //    return;

            //if (text.Length > maxWidth)
            //    text = text.Substring(0, maxWidth);

            //Console.SetCursorPosition(x, y);
            //Console.Write(text);

            // проверка по локальным координатам
            if (localY < 0 || localY >= InnerHeight)
                return;

            if (localX < 0 || localX >= InnerWidth)
                return;

            int maxWidth = InnerWidth - localX;
            if (maxWidth <= 0)
                return;

            if (text.Length > maxWidth)
                text = text.Substring(0, maxWidth);

            // перевод в глобальные координаты
            int globalX = InnerX + localX;
            int globalY = InnerY + localY;

            Console.SetCursorPosition(globalX, globalY);
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
