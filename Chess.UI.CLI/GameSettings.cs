namespace Chess.UI.CLI
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
        public PlayerType WhitePlayer { get; set; }
        public PlayerType BlackPlayer { get; set; }

        public int AiDifficulty { get; set; } = 1;
    }
}
