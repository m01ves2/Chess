namespace Chess.Application.Models
{ 
    //public enum GameMode
    //{
    //    HumanVsHuman,
    //    HumanVsAi,
    //    AiVsAi,
    //};

    public enum PlayerType
    {
        Human,
        Ai,
    }

    public class GameSettings
    {
        //public GameMode Mode { get; set; }
        public PlayerType WhitePlayer { get; set; } = PlayerType.Human;
        public PlayerType BlackPlayer { get; set; } = PlayerType.Human;

        public int AiDifficulty { get; set; } = 1;
    }
}
