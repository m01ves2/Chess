using Chess.Application;
using Chess.Domain;

namespace Chess.UI.CLI.Panels
{
    public abstract class TextPanelBase : BasePanel
    {
        protected List<string> _lines = new();

        public TextPanelBase(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }
        //public override void BuildBuffer()
        //{
        //    List<string> strings = new List<string>() { "hello", "world", "Is", "anybody", "here^?" };
        //    for(int i = 0; i < strings.Count; i++) {
        //        AddLine(strings[i]);
        //    }
        //}


        protected void AddLine(string line)
        {
            _lines.Add(line);
        }

        public override void Flush()
        {
            int visible = InnerHeight;

            var visibleLines = _lines.Skip(Math.Max(0, _lines.Count - visible)).Take(visible);
            int row = 0;
            foreach (var line in visibleLines) {
                //Console.SetCursorPosition(InnerX, InnerY + row);
                //Console.Write(line.PadRight(InnerWidth));
                SafeWrite(InnerX, InnerY + row, line.PadRight(InnerWidth));
                row++;
            }

            for(int emptyRow = visibleLines.Count(); emptyRow < InnerHeight; emptyRow++) {
                Console.SetCursorPosition(InnerX, InnerY + emptyRow);
                Console.Write(new string(' ', InnerWidth));
            }
        }

        protected void ClearLines()
        {
            _lines.Clear();
        }
    }
}
