using System.Diagnostics;

namespace Chess.UI.CLI.Diagnostics
{
    public class ScreenDebugger
    {
        private Stopwatch _stopwatch = new Stopwatch();
        private int _frameCount = 0;
        private int _fps = 0;

        public ScreenDebugger()
        {
            _stopwatch.Start();
        }

        // Вызываем внутри Flush() или CopyToScreen()
        public void Tick()
        {
            _frameCount++;

            if (_stopwatch.ElapsedMilliseconds >= 1000) {
                _fps = _frameCount;
                _frameCount = 0;
                _stopwatch.Restart();

                // Пишем FPS в левый верхний угол
                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"FPS: {_fps}   "); // пробелы, чтобы стирать старое значение
            }
        }
    }
}
