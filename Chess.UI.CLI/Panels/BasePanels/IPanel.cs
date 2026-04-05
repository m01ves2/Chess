using Chess.UI.CLI.Panels.BasePanels.Rendering;

namespace Chess.UI.CLI.Panels.BasePanels
{
    public interface IPanel
    {
        void Render();
        void DrawBorder(CellRender[,] screen);
        void CopyToScreen(CellRender[,] screen);
    }
}
