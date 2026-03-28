using Chess.Application.ViewModels;
using Chess.UI.CLI.Panels.BasePanels;
using Chess.UI.CLI.Panels.BasePanels.Rendering;
using Chess.UI.CLI.Views;

namespace Chess.UI.CLI.Panels
{
    public class MessagePanel : TextPanelBase<MessageView>
    {
        public MessagePanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }

        public override void BuildBuffer(MessageView view)
        {
            //List<string> strings = new List<string>() { "You piece is being promoted!", "Choose piece - ", "Q - queen", "K - knight", "R - rook", "B - bishop:"};
            //ClearLines();
            //for (int i = 0; i < strings.Count; i++) {
            //    AddLine(strings[i]);
            //}
            ClearLines();
            
            if(view.Type != MessageType.None) {
                AddLine(new LineRender() { Text = view.Text, Style = LineStyle.None });
            }
        }
    }
}
