using Chess.Application;
using Chess.Domain;

namespace Chess.UI.CLI.Panels
{
    public class MessagePanel : TextPanelBase
    {
        public MessagePanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }

        public override void BuildBuffer()
        {
            List<string> strings = new List<string>() { "You piece is being promoted!", "Choose piece (Q - queen, K - knight, R - rook, B - bishop): " };
            ClearLines();
            for (int i = 0; i < strings.Count; i++) {
                AddLine(strings[i]);
            }
        }
    }
}
