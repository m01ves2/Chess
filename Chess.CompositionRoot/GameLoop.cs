namespace Chess.CompositionRoot
{
    public enum GameStatus
    {
        Playing,
        Navigation,
    };

    public class GameLoop
    {
        //private readonly ScreenManager _screenManager;
        //private GameController _gameController;
        //private readonly IInputHandler _inputHandler;
        //private IPlayer _whitePlayer;
        //private IPlayer _blackPlayer;

        //private GameStatus _gameStatus = GameStatus.Navigation;
        //public GameLoop(GameController gameController, ScreenManager screenManager, IInputHandler input)
        //{
        //    _gameController = gameController;
        //    _screenManager = screenManager;
        //    _inputHandler = input;

        //    //_whitePlayer = new HumanPlayer(_gameController, screenManager, input);
        //    //_blackPlayer = new AiPlayer(_gameController);
        //}

        //public void Run()
        //{
        //    while (/*!_gameController.IsGameOver() ||*/ !_screenManager.IsExitRequested) {
        //        _screenManager.Render();
        //        switch (_gameStatus) {
        //            case GameStatus.Navigation:
        //                RunNavigationStep();
        //                break;

        //            case GameStatus.Playing:
        //                RunGameStep();
        //                break;
        //        }

        //    }
        //}

        //private void RunNavigationStep()
        //{
        //    //_screenManager.Render();
        //    var action = _inputHandler.ReadAction();
        //    _screenManager.HandleInput(action);

        //    if (_screenManager.IsStartGameRequested) {
        //        StartNewGame();
        //    }
        //}

        //private void StartNewGame()
        //{
        //    //_whitePlayer = new HumanPlayer(_gameController, _screenManager, _inputHandler);
        //    //_blackPlayer = new AiPlayer(_gameController);
        //    //_gameStatus = GameStatus.Playing;

        //    _gameController = new GameController();

        //    _screenManager.SetScreen(new GameScreen(_screenManager, _gameController));

        //    //_whitePlayer = new HumanPlayer(_gameController, _screenManager, _inputHandler);
        //    //_blackPlayer = new HumanPlayer(_gameController, _screenManager, _inputHandler);
        //    _whitePlayer = new AiPlayer(_gameController);
        //    _blackPlayer = new AiPlayer(_gameController);

        //    _gameStatus = GameStatus.Playing;
        //}

        //private void RunGameStep()
        //{
        //    try {
        //        var player = GetCurrentPlayer(_gameController.GamePosition);
        //        var moves = _gameController.GetAllLegalMoves();
        //        var move = player.ChooseMove(_gameController.GamePosition, moves);
        //        _gameController.DoMove(move);
        //        //_gameController._pendingMove = null;
        //    }
        //    catch (ExitGameException ex) {
        //        _gameStatus = GameStatus.Navigation;
        //    }

        //    if (_gameController.IsGameOver) {
        //        _gameStatus = GameStatus.Navigation; // или GameOver
        //    }
        //}

        //private IPlayer GetCurrentPlayer(GamePosition gamePosition)
        //{
        //    return gamePosition.CurrentPlayer == PieceColor.White ? _whitePlayer : _blackPlayer;
        //}
    }
}
