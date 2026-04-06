namespace Chess.UI.CLI.Themes
{
    public static class Theme
    {
        public static class Text
        {
            public const ConsoleColor Default = ConsoleColor.White;
            public const ConsoleColor Selected = ConsoleColor.Red;
        }

        public static class Background
        {
            public const ConsoleColor Default = ConsoleColor.Black;
            public const ConsoleColor BoardLight = ConsoleColor.Gray;
            public const ConsoleColor BoardDark = ConsoleColor.DarkGray;
            public const ConsoleColor Highlight = ConsoleColor.DarkYellow;
            public const ConsoleColor Selected = ConsoleColor.DarkGreen;
        }
    }
}
