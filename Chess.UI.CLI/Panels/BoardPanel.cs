using Chess.Application;
using Chess.Domain;

namespace Chess.UI.CLI.Panels
{
    public class BoardPanel : BasePanel
    {
        public BoardPanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }

        public override void BuildBuffer(GamePosition position, GameScene scene)
        {
            for (int row = 0; row < InnerHeight; row++) {
                _buffer[row] = new string('B', InnerWidth);
            }
        }
    }
}
