using Chess.UI.CLI.Diagnostics;
using Chess.UI.CLI.Models;
using Chess.UI.CLI.Panels.BasePanels;
using Chess.UI.CLI.Panels.BasePanels.Rendering;
using Chess.UI.CLI.Themes;

namespace Chess.UI.CLI.Screens.BaseScreens
{
    public abstract class BaseScreen
    {
        protected const int Width = 80;
        protected const int Height = 40;
        protected CellRender[,] _screenBuffer;
        protected CellRender[,] _prevScreenBuffer;

        protected readonly ScreenManager _manager;
        private int ConsoleWidth = Console.WindowWidth;
        private int ConsoleHeight = Console.WindowHeight;

        private static readonly ConsoleColor DefaultFg = Theme.Text.Default;
        private static readonly ConsoleColor DefaultBg = Theme.Background.Default;
        private static ScreenDebugger _debugger = new ScreenDebugger();

        // все панели экрана
        protected List<IPanel> _panels = new();

        // Координаты курсора

        public BaseScreen(ScreenManager manager)
        {
            _manager = manager;

            _screenBuffer = new CellRender[Height, Width];
            _prevScreenBuffer = new CellRender[Height, Width];

            for (int row = 0; row < Height; row++)
                for (int col = 0; col < Width; col++) {
                    _screenBuffer[row, col] = new CellRender();
                    _screenBuffer[row, col].Symbol = ' ';
                    //_screenBuffer[row, col].fg = Console.ForegroundColor;
                    //_screenBuffer[row, col].bg = Console.BackgroundColor;

                    _prevScreenBuffer[row, col] = new CellRender();
                    _prevScreenBuffer[row, col].Symbol = ' ';
                    //_prevScreenBuffer[row, col].bg = Console.BackgroundColor;
                    //_prevScreenBuffer[row, col].fg = Console.ForegroundColor;
                }

            //Init();
        }

        public void Render()
        {
            Clear();
            BuildScreen();
            CopyToScreen();
            Flush();
        }

        public void Clear()
        {
            for (int row = 0; row < Height; row++)
                for (int col = 0; col < Width; col++) {
                    _screenBuffer[row, col].Symbol = ' ';
                    _screenBuffer[row, col].bg = DefaultBg;
                    _screenBuffer[row, col].fg = DefaultFg;
                }
        }
        public abstract void BuildScreen();
        public void CopyToScreen()
        {
            foreach (var panel in _panels) {
                panel.Render();
                panel.DrawBorder(_screenBuffer);
                panel.CopyToScreen(_screenBuffer);
            }
        }

        //public void Flush()
        //{
        //    var defaultForeground = Console.ForegroundColor;
        //    var defaultBackground = Console.BackgroundColor;
        //    for (int row = 0; row < Height; row++) {
        //        string rowChanged = "";
        //        int rowChangedStartIndex = 0;

        //        ConsoleColor segmentFg = defaultForeground;
        //        ConsoleColor segmentBg = defaultBackground;

        //        for (int col = 0; col < Width; col++) {
        //            var current = _screenBuffer[row, col];
        //            var prev = _prevScreenBuffer[row, col];

        //            if (current != prev) {
        //                if (string.IsNullOrEmpty(rowChanged)) {
        //                    rowChangedStartIndex = col;
        //                    segmentFg = current.fg;
        //                    segmentBg = current.bg;
        //                }
        //                else if (current.fg != segmentFg || current.bg != segmentBg) {
        //                    FlushSegment(row, rowChangedStartIndex, rowChanged, segmentFg, segmentBg); //старый сегмент закрыть и начать новый
        //                    rowChanged = "";
        //                    rowChangedStartIndex = col;
        //                    segmentFg = current.fg;
        //                    segmentBg = current.bg;
        //                }
        //                rowChanged += current.Symbol;
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(rowChanged)) {
        //            FlushSegment(row, rowChangedStartIndex, rowChanged, segmentFg, segmentBg);
        //        }
        //    }
        //    Console.BackgroundColor = defaultBackground;
        //    Console.ForegroundColor = defaultForeground;
        //}

        //void FlushSegment(int row, int start, string text, ConsoleColor fg, ConsoleColor bg)
        //{
        //    Console.SetCursorPosition(start, row);
        //    Console.ForegroundColor = fg;
        //    Console.BackgroundColor = bg;
        //    Console.Write(text);

        //    // обновляем prev buffer
        //    for (int i = 0; i < text.Length; i++) {
        //        var c = _screenBuffer[row, start + i];
        //        var p = _prevScreenBuffer[row, start + i];

        //        p.Symbol = c.Symbol;
        //        p.fg = c.fg;
        //        p.bg = c.bg;
        //    }
        //}



        public void Flush()
        {
            var defaultForeground = Theme.Text.Default;
            var defaultBackground = Theme.Background.Default;

            ConsoleColor currentFg = defaultForeground;
            ConsoleColor currentBg = defaultBackground;

            Console.SetCursorPosition(0, 0); // всегда в верхний левый угол
            for (int row = 0; row < Height; row++) {
                for (int col = 0; col < Width; col++) {
                    var current = _screenBuffer[row, col];
                    var previous = _prevScreenBuffer[row, col];

                    if (current != previous) {
                        Console.SetCursorPosition(col, row);

                        if (current.fg != currentFg) {
                            Console.ForegroundColor = current.fg;
                            currentFg = current.fg;
                        }

                        if (current.bg != currentBg) {
                            Console.BackgroundColor = current.bg;
                            currentBg = current.bg;
                        }

                        Console.Write(current.Symbol);

                        // Копируем значение в prev
                        _prevScreenBuffer[row, col] = current;
                    }
                }
            }

            _debugger.Tick();

            Console.ForegroundColor = defaultForeground;
            Console.BackgroundColor = defaultBackground;
        }

        public abstract bool HandleInput(PlayerAction action);

        //public void ConsoleResize()
        //{
        //    if (Console.WindowHeight != ConsoleHeight || Console.WindowWidth != ConsoleWidth)
        //        Init();
        //}

        //protected virtual void Init()
        //{
        //}

        public virtual void Tick()
        {
            //Nothing to do
        }
    }
}
