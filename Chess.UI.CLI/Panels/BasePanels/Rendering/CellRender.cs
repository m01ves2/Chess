using Chess.Domain;
using System.Numerics;

namespace Chess.UI.CLI.Panels.BasePanels.Rendering
{
    public struct CellRender
    {
        public char Symbol { get; set; }
        public ConsoleColor fg { get; set; }
        public ConsoleColor bg { get; set; }

        public override bool Equals(object? obj) => obj is CellRender other && this == other;

        public static bool operator== (CellRender left, CellRender right)
        {
            return left.bg == right.bg && left.fg == right.fg && left.Symbol == right.Symbol;
        }

        public static bool operator!=(CellRender left, CellRender right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
