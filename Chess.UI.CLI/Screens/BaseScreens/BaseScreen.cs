using Chess.Application;
using Chess.UI.CLI.Models;
using Chess.UI.CLI.Panels.BasePanels.Rendering;
using System;

namespace Chess.UI.CLI.Screens.BaseScreens
{
    public abstract class BaseScreen
    {
        protected const int MinWidth = 80;
        protected const int MinHeight = 40;
        protected CellRender[,] _screenBuffer;
        protected CellRender[,] _prevScreenBuffer;

        protected readonly ScreenManager _manager;
        private int ConsoleWidth = Console.WindowWidth;
        private int ConsoleHeight = Console.WindowHeight;

        public BaseScreen(ScreenManager manager)
        {
            _manager = manager;

            _screenBuffer = new CellRender[MinHeight, MinWidth];
            _prevScreenBuffer = new CellRender[MinHeight, MinWidth];

            for (int row = 0; row < MinHeight; row++)
                for (int col = 0; col < MinWidth; col++) {
                    _screenBuffer[row, col] = new CellRender();
                    _screenBuffer[row, col].Symbol = ' ';

                    _prevScreenBuffer[row, col] = new CellRender();
                    _prevScreenBuffer[row, col].Symbol = ' ';
                }

            Init();
        }

        public void Render()
        {
            //Console.Clear();
            BuildScreen();
            Flush();
        }

        public abstract void BuildScreen();
        public abstract bool HandleInput(PlayerAction action);


        //public void Flush()
        //{
        //    var defaultForeground = Console.ForegroundColor;
        //    var defaultBackground = Console.BackgroundColor;
        //    for (int row = 0; row < MinHeight; row++) {
        //        string rowChanged = "";
        //        int rowChangedStartIndex = 0;

        //        ConsoleColor segmentFg = defaultForeground;
        //        ConsoleColor segmentBg = defaultBackground;

        //        for (int col = 0; col < MinWidth; col++) {
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


        //public void Flush()
        //{
        //    Console.SetCursorPosition(0, 0);

        //    for (int row = 0; row < MinHeight; row++) {
        //        for (int col = 0; col < MinWidth; col++) {
        //            var cell = _screenBuffer[row, col];
        //            Console.ForegroundColor = cell.fg;
        //            Console.BackgroundColor = cell.bg;
        //            Console.Write(cell.Symbol);
        //        }
        //    }
        //}

        public void Flush()
        {
            var defaultbg = Console.BackgroundColor;
            var defaultfg = Console.ForegroundColor;
            for (int row = 0; row < MinHeight; row++) {
                for (int col = 0; col < MinWidth; col++) {
                    if (_prevScreenBuffer[row, col] != _screenBuffer[row, col]) {
                        Console.SetCursorPosition(col, row);
                        Console.ForegroundColor = _screenBuffer[row, col].fg;
                        Console.BackgroundColor = _screenBuffer[row, col].bg;
                        Console.Write(_screenBuffer[row, col].Symbol);

                        _prevScreenBuffer[row, col].Symbol = _screenBuffer[row, col].Symbol;
                        _prevScreenBuffer[row, col].fg = _screenBuffer[row, col].fg;
                        _prevScreenBuffer[row, col].bg = _screenBuffer[row, col].bg;
                    }
                }
            }
            Console.BackgroundColor = defaultbg;
            Console.ForegroundColor = defaultfg;
            ClearScreenBuffer();
        }

        public void ClearScreenBuffer()
        {
            for (int row = 0; row < MinHeight; row++)
                for (int col = 0; col < MinWidth; col++) {
                    _screenBuffer[row, col].Symbol = ' ';
                    _screenBuffer[row, col].bg = Console.BackgroundColor;
                    _screenBuffer[row, col].fg = Console.BackgroundColor;
                }
        }


        public void ConsoleResize()
        {
            if (Console.WindowHeight != ConsoleHeight || Console.WindowWidth != ConsoleWidth)
                Init();
        }

        protected virtual void Init()
        {
        }

        public virtual void Tick()
        {
            //Nothing to do
        }
    }
}
