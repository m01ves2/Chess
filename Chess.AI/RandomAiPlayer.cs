using Chess.Domain;

namespace Chess.AI
{
    public class RandomAiPlayer
    {
        private readonly Random _random = new Random();

        public Move ChooseMove(GamePosition position, IEnumerable<Move> moves)
        {
            var moveList = moves.ToList();
            if (!moveList.Any())
                throw new Exception("No moves available");

            return moveList[_random.Next(moveList.Count)];
        }
    }
}
