namespace Chess.UI.CLI.Panels.BasePanels.Rendering
{
    [Flags]
    public enum LineStyle
    {
        None = 0,
        Selected = 1 << 0,
        Highlighted = 1 << 1,
        Error = 1 << 2,
        Italic = 1 << 3,
        Bold = 1 << 4
    }
    public class LineRender
    {
        public string Text { get; set; }
        public LineStyle Style { get; set; } = LineStyle.None;  // стиль строки (цвет, жирность, курсив, подсветка)
    }
}
