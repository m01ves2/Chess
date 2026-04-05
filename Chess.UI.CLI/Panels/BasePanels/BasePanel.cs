using Chess.Application.Views;
using Chess.UI.CLI.Panels.BasePanels.Rendering;

namespace Chess.UI.CLI.Panels.BasePanels
{
    public abstract class BasePanel<ViewType> : IPanel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int InnerWidth => Width - 2;
        public int InnerHeight => Height - 2; 
        public int InnerX => X + 1;
        public int InnerY => Y + 1;

        protected ViewType _view;

        public BasePanel(int x, int y, int width, int height) //координаты и размер панели
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;

            ClearPanel(); //очистка пространства для новорождённой панели
        }

        public void Render()
        {
            ClearBuffer();
            BuildBuffer(_view);      // заполняем _buffer
        }

        public void SetView(ViewType view)
        {
            _view = view;
        }

        public abstract void BuildBuffer(ViewType view); //_buffer
        public abstract void CopyToScreen(CellRender[,] _screenBuffer);

        public void DrawBorder(CellRender[,] _screenBuffer)
        {

            // верх
            _screenBuffer[Y, X].Symbol = '┌';
            _screenBuffer[Y, X].bg = ConsoleColor.Black;
            _screenBuffer[Y, X].fg = ConsoleColor.White;
            for (int i = 1; i < Width - 1; i++) {
                _screenBuffer[Y, X + i].Symbol = '─';
                _screenBuffer[Y, X + i].bg = ConsoleColor.Black;
                _screenBuffer[Y, X + i].fg = ConsoleColor.White;
            }
            _screenBuffer[Y, X + Width - 1].Symbol = '┐';
            _screenBuffer[Y, X + Width - 1].bg = ConsoleColor.Black;
            _screenBuffer[Y, X + Width - 1].fg = ConsoleColor.White;

            // низ
            _screenBuffer[Y + Height - 1, X].Symbol = '└';
            _screenBuffer[Y + Height - 1, X].bg = ConsoleColor.Black;
            _screenBuffer[Y + Height - 1, X].fg = ConsoleColor.White;
            for (int i = 1; i < Width - 1; i++) {
                _screenBuffer[Y + Height - 1, X + i].Symbol = '─';
                _screenBuffer[Y + Height - 1, X + i].bg = ConsoleColor.Black;
                _screenBuffer[Y + Height - 1, X + i].fg = ConsoleColor.White;

            }
            _screenBuffer[Y + Height - 1, X + Width - 1].Symbol = '┘';
            _screenBuffer[Y + Height - 1, X + Width - 1].bg = ConsoleColor.Black;
            _screenBuffer[Y + Height - 1, X + Width - 1].fg = ConsoleColor.White;

            // боковые
            for (int i = 1; i < Height - 1; i++) {
                _screenBuffer[Y + i, X].Symbol = '│';
                _screenBuffer[Y + i, X].bg = ConsoleColor.Black;
                _screenBuffer[Y + i, X].fg = ConsoleColor.White;

                _screenBuffer[Y + i, X + Width - 1].Symbol = '│';
                _screenBuffer[Y + i, X + Width - 1].bg = ConsoleColor.Black;
                _screenBuffer[Y + i, X + Width - 1].fg = ConsoleColor.White;
            }
        }

        //public void DrawBorder()
        //{
        //    // верх
        //    WriteAt(X, Y, "┌" + new string('─', Math.Max(0, Width - 2)) + "┐");

        //    // низ
        //    WriteAt(X, Y + Height - 1, "└" + new string('─', Math.Max(0, Width - 2)) + "┘");

        //    // боковые
        //    for (int i = 1; i < Height - 1; i++) {
        //        WriteAt(X, Y + i, "│");
        //        WriteAt(X + Width - 1, Y + i, "│");
        //    }
        //}

        //protected void WriteAt(int x, int y, string text)
        //{
        //    Console.SetCursorPosition(x, y);
        //    Console.Write(text);
        //}

        //protected void SafeWrite(int localX, int localY, string text, LineStyle style = LineStyle.None)
        //{
        //    // проверка по локальным координатам
        //    if (localY < 0 || localY >= InnerHeight)
        //        return;

        //    if (localX < 0 || localX >= InnerWidth)
        //        return;

        //    int maxWidth = InnerWidth - localX;
        //    if (maxWidth <= 0)
        //        return;

        //    if (text.Length > maxWidth)
        //        text = text.Substring(0, maxWidth);

        //    // перевод в глобальные координаты
        //    int globalX = InnerX + localX;
        //    int globalY = InnerY + localY;

        //    Console.SetCursorPosition(globalX, globalY);
        //    if (style == LineStyle.Selected) //TODO
        //        PrintSelected(text);
        //    else
        //        PrintNormal(text);       
        //}

        public void ClearPanel()
        {
            for (int row = 0; row < InnerHeight; row++) {
                Console.SetCursorPosition(InnerX, InnerY + row);
                //Console.BackgroundColor = ConsoleColor.Black;
                //Console.ForegroundColor = ConsoleColor.White;
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

        protected static char GetSymbol(PieceViewType type, PieceViewColor color)
        {
            return (type, color) switch
            {
                (PieceViewType.Rook, PieceViewColor.White) => '\u2656',
                (PieceViewType.Rook, PieceViewColor.Black) => '\u265C',
                (PieceViewType.Knight, PieceViewColor.White) => '\u2658',
                (PieceViewType.Knight, PieceViewColor.Black) => '\u265E',
                (PieceViewType.Bishop, PieceViewColor.White) => '\u2657',
                (PieceViewType.Bishop, PieceViewColor.Black) => '\u265D',
                (PieceViewType.Queen, PieceViewColor.White) => '\u2655',
                (PieceViewType.Queen, PieceViewColor.Black) => '\u265B',
                (PieceViewType.King, PieceViewColor.White) => '\u2654',
                (PieceViewType.King, PieceViewColor.Black) => '\u265A',
                (PieceViewType.Pawn, PieceViewColor.White) => '\u2659',
                (PieceViewType.Pawn, PieceViewColor.Black) => '\u265F',
                _ => '?'
            };
        }

        public abstract void ClearBuffer();
    }
}
