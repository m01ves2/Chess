using Chess.Application.ViewModels;

namespace Chess.UI.CLI.Panels
{
    public class InfoPanel : TextPanelBase
    {
        private InfoViewModel _infoVM;
        public InfoPanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }

        public override void BuildBuffer()
        {
            ClearLines();
            foreach(var item in _infoVM.Info)
                AddLine(item);
        }

        public void SetData(InfoViewModel infoVM)
        {
            _infoVM = infoVM;
        }
    }
}
