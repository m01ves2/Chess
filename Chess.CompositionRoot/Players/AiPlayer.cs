using Chess.Application;
using Chess.Domain;

namespace Chess.CompositionRoot.Players
{
    public class AiPlayer : IPlayer
    {
        private readonly Random _random = new Random();

        public AiPlayer(GameController gameController) { }
        public Move ChooseMove(GamePosition position, IEnumerable<Move> moves)
        {
            var moveList = moves.ToList();
            if (!moveList.Any())
                throw new Exception("No moves available"); //TODO GameOver??

            Thread.Sleep(1000);
            return moveList[_random.Next(moveList.Count)];

        }
    }
}
