using Chess.Application.Interfaces;

namespace Chess.Application
{
    public class GameLoop
    {
        private readonly GameController _controller;
        private readonly IBoardRenderer _renderer;
        private readonly IInputHandler _input;

        public GameLoop(GameController controller, IBoardRenderer renderer, IInputHandler input)
        {
            _controller = controller;
            _renderer = renderer;
            _input = input;
        }

        public void Run()
        {
            while (!_controller.IsGameOver()) {
                _renderer.Render(_controller.CurrentGameState);
                var action = _input.ReadAction();
                _controller.ProcessAction(action);
            }
        }
    }
}
