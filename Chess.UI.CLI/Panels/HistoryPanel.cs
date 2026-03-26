using Chess.Application;
using Chess.Application.ViewModels;
using Chess.Domain;

namespace Chess.UI.CLI.Panels
{
    public class HistoryPanel : TextPanelBase
    {
        private MoveHistoryViewModel _historyVM;
        public HistoryPanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
            _historyVM = new MoveHistoryViewModel();
        }

        public override void BuildBuffer()
        {
            ClearLines();
            for (int i = 0; i < _historyVM.MoveHistory.Count; i++) {
                AddLine(_historyVM.MoveHistory[i]);
            }
        }

        public void SetData(MoveHistoryViewModel historyVM)
        {
            _historyVM = historyVM;
        }

        //public override void Render()
        //{
        //    Clear();

        //    foreach (var move in _moves)
        //        AddLine(move);

        //    RenderLines();
        //}


        ////private IReadOnlyList<string> _moves;
        ////private int _scrollOffset = 0;

        ////public HistoryPanel(int x, int y, int w, int h) : base(x, y, w, h) { }

        ////public void SetMoves(IReadOnlyList<string> moves)
        ////{
        ////    _moves = moves;
        ////}

        ////protected override void BuildBuffer()
        ////{
        ////    // очистка буфера
        ////    for (int i = 0; i < Height; i++)
        ////        _buffer[i] = new string(' ', Width);

        ////    int visible = Height;
        ////    var visibleMoves = _moves
        ////        .Skip(_scrollOffset)
        ////        .Take(visible)
        ////        .ToList();

        ////    for (int i = 0; i < visibleMoves.Count; i++) {
        ////        string line = visibleMoves[i];

        ////        if (line.Length > Width)
        ////            line = line.Substring(0, Width);

        ////        _buffer[i] = line.PadRight(Width);
        ////    }
        ////}

        ////public void ScrollUp()
        ////{
        ////    if (_scrollOffset > 0)
        ////        _scrollOffset--;
        ////}

        ////public void ScrollDown()
        ////{
        ////    if (_scrollOffset < _moves.Count - Height)
        ////        _scrollOffset++;
        ////}

        //private List<string> _moves = new List<string>();
        //private int _scrollOffset = 0;

        //public HistoryPanel(int x, int y, int width, int height) : base(x, y, width, height)
        //{
        //    for (int i = 0; i < 100; i++) {
        //        _moves.Add(i.ToString());
        //    }
        //}

        //public void SetMoves(List<string> moves)
        //{
        //    _moves = moves;
        //}

        //public override void BuildBuffer(GamePosition position, GameScene scene)
        //{
        //    //for (int row = 1; row < Height - 1; row++) {
        //    //    //_buffer[row] = new string('H', Width - 1);
        //    //    _buffer[row]
        //    //}
        //    //Clear();
        //    int visible = InnerHeight;
        //    var visibleMoves = _moves.Skip(_scrollOffset).Take(visible).ToList();

        //    for (int i = 0; i < InnerHeight; i++) {
        //        if (i < visibleMoves.Count) {
        //            string line = visibleMoves[i];

        //            if (line.Length > InnerWidth)
        //                line = line.Substring(0, InnerWidth);

        //            _buffer[i] = line.PadRight(InnerWidth);
        //        }
        //        else {
        //            _buffer[i] = new string(' ', InnerWidth);
        //        }
        //    }

        //}
    }
}
