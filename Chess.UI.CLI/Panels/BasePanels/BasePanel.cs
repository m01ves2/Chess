using Chess.Application;
using Chess.Domain;
using Chess.UI.CLI.Panels.BasePanels.Rendering;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

namespace Chess.UI.CLI.Panels.BasePanels
{
    public abstract class BasePanel<ViewType>
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

        public void Render(ViewType view)
        {
            DrawBorder();
            BuildBuffer(view);      // заполняем _buffer
            Flush();            // выводим только изменения
        }

        public abstract void BuildBuffer(ViewType view); //_buffer
        public abstract void Flush();
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

        protected void SafeWrite(int localX, int localY, string text, LineStyle style = LineStyle.None)
        {
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
            if (style == LineStyle.Selected) //TODO
                PrintSelected(text);
            else
                PrintNormal(text);       
        }

        public void ClearPanel()
        {
            for (int row = 0; row < InnerHeight; row++) {
                Console.SetCursorPosition(InnerX, InnerY + row);
                Console.Write(new string(' ', InnerWidth));
            }
        }

        protected void PrintSelected(string text)
        {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = defaultColor;
        }

        protected void PrintNormal(string text)
        {
            Console.WriteLine(text);
        }
    }
}
