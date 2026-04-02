using Chess.Application.Models;
using Chess.Application.Players;
using Chess.Domain;

namespace Chess.Application
{
    public class GameChess
    {
        private GameController _controller;
        private IPlayer _whitePlayer;
        private IPlayer _blackPlayer;
        private IPlayer _currentPlayer;
        private DateTime _lastMoveTime = DateTime.MinValue;
        private TimeSpan _aiDelay = TimeSpan.FromMilliseconds(300);

        public GameChess(GameController controller, GameSettings gameSettings)
        {
            _controller = controller;

            if(gameSettings.WhitePlayer == PlayerType.Human)
                _whitePlayer = new HumanPlayer(_controller); //потом отдельно отрегулируем Ai va Ai
            else
                _whitePlayer = new AiPlayer(_controller, gameSettings.AiDifficulty);

            if (gameSettings.BlackPlayer == PlayerType.Human)
                _blackPlayer = new HumanPlayer(_controller); //потом отдельно отрегулируем Ai va Ai
            else
                _blackPlayer = new AiPlayer(_controller, gameSettings.AiDifficulty);

            _currentPlayer = _whitePlayer;
        }

        public void Tick()
        {
            if (_currentPlayer is AiPlayer) {
                if (DateTime.Now - _lastMoveTime < _aiDelay)
                    return;

                _lastMoveTime = DateTime.Now;
            }


            var moves = _controller.GetAllLegalMoves().ToList();
            if (!moves.Any())
                throw new Exception("No moves available"); //TODO GameOver??

            var move = _currentPlayer.TryGetMove(moves );

            if (move != null) {
                _controller.DoMove(move);
                SwitchPlayer();
            }
        }

        public void Select(int row, int col)
        {
            if(_currentPlayer is HumanPlayer hp)
                hp.Select(row, col);
        }

        public void SwitchPlayer()
        {
            _currentPlayer = (_currentPlayer == _whitePlayer) ? _blackPlayer : _whitePlayer;
        }
    }
}
