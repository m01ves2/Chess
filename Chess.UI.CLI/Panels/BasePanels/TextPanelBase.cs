using Chess.Application;
using Chess.Domain;
using Chess.UI.CLI.Panels.BasePanels.Rendering;
using Chess.UI.CLI.Themes;
using static System.Net.Mime.MediaTypeNames;

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
            //line.Text = line.Text.PadRight(InnerWidth);
            _lines.Add(line);
        }


        public override void CopyToScreen(CellRender[,] _screenBuffer)
        {

            int visible = InnerHeight;
            var visibleLines = _lines.Skip(Math.Max(0, _lines.Count - visible)).Take(visible).ToList();
            for (int y = 0; y < visibleLines.Count; y++) {
                var line = visibleLines[y];

                for (int x = 0; x < InnerWidth; x++) {
                    char symbol = x < line.Text.Length ? line.Text[x] : ' ';

                    var fg = line.Style == LineStyle.Selected ? Theme.Text.Selected : Theme.Text.Default; 
                    var bg = Theme.Background.Default;


                    _screenBuffer[Y + 1 + y, X + 1 + x].Symbol = symbol;
                    _screenBuffer[Y + 1 + y, X + 1 + x].fg = fg;
                    _screenBuffer[Y + 1 + y, X + 1 + x].bg = bg;
                }
            }

            for (int y = visibleLines.Count; y < InnerHeight; y++) {
                for (int x = 0; x < InnerWidth; x++) {
                    var cell = _screenBuffer[Y + 1 + y, X + 1 + x];
                    cell.Symbol = ' ';
                    cell.fg = Theme.Text.Default;
                    cell.bg = Theme.Background.Default;
                }
            }
        }

        public override void ClearBuffer()
        {
            _lines.Clear();
        }
    }
}
