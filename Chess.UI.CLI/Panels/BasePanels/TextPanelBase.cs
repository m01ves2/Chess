using Chess.Application;
using Chess.Domain;
using Chess.UI.CLI.Panels.BasePanels.Rendering;

namespace Chess.UI.CLI.Panels.BasePanels
{
    public abstract class TextPanelBase<ViewType> : BasePanel<ViewType>
    {
        private List<LineRender> _lines = new List<LineRender>();

        public TextPanelBase(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }

        protected void AddLine(LineRender line)
        {
            _lines.Add(line);
        }

        public override void Flush()
        {
            int visible = InnerHeight;

            var visibleLines = _lines.Skip(Math.Max(0, _lines.Count - visible)).Take(visible);
            int row = 0;
            foreach (var line in visibleLines) {
                SafeWrite(0, row, line.Text.PadRight(InnerWidth), line.Style);
                row++;
            }

            for(int emptyRow = visibleLines.Count(); emptyRow < InnerHeight; emptyRow++) {
                //Console.SetCursorPosition(InnerX, InnerY + emptyRow);
                //Console.Write(new string(' ', InnerWidth));
                SafeWrite(0, Y + emptyRow, new string(' ', InnerWidth));
            }
        }

        protected void ClearLines()
        {
            _lines.Clear();
        }
    }
}
