using Chess.Domain;
using System.Numerics;

namespace Chess.UI.CLI.Panels.BasePanels.Rendering
{
    public class CellRender
    {
        public char Symbol { get; set; }
        public ConsoleColor fg { get; set; }
        public ConsoleColor bg { get; set; }

        public override bool Equals(object? obj) => obj is CellRender other && this == other;

        public static bool operator== (CellRender left, CellRender right)
        {
            return left.Symbol == right.Symbol && left.fg == right.fg && left.bg == right.bg;
        }

        public static bool operator!=(CellRender left, CellRender right)
        {
            return !(left == right);
        }
    }
}
