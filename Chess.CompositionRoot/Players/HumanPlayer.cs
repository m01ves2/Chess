using Chess.Application;
using Chess.CompositionRoot.Exceptions;
using Chess.Domain;
using Chess.UI.CLI;
using Chess.UI.CLI.Interfaces;

namespace Chess.CompositionRoot.Players
{
    public class HumanPlayer : IPlayer
    {
        private readonly ScreenManager _screenManager;
        private readonly GameController _gameController;
        private readonly IInputHandler _input;
        public HumanPlayer(GameController gameController, ScreenManager screenManager, IInputHandler input) { 
            _gameController = gameController;
            _screenManager = screenManager;
            _input = input;
        }

        public Move ChooseMove(GamePosition position, IEnumerable<Move> moves)
        {
            while (true) {
                var action = _input.ReadAction();

                _screenManager.HandleInput(action);

                //var move = _gameController.GetPendingMove();

                var move = _gameController.TryGetPendingMove();
                if (move != null && IsValid(move, moves)) {
                    return move;
                }

                if (_screenManager.IsExitRequested)
                    throw new ExitGameException();

            }
        }

        private bool IsValid(Move move, IEnumerable<Move> moves)
        {
            return moves.Any(m => m.From == move.From && m.To == m.To);
        }
    }
}
