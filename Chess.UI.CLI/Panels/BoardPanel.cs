using Chess.Application;
using Chess.Domain;

namespace Chess.UI.CLI.Panels
{
    public class BoardPanel : GraphicsPanelBase
    {
        public BoardPanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }

        public override void BuildBuffer()
        {
            //TODO
            for (int row = 0; row < InnerHeight; row++) {
                for(int col = 0; col < InnerWidth; col++)
                _buffer[row, col] = 'B';
            }
        }
    }
}
