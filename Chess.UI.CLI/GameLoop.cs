using Chess.Application.Interfaces;
using Chess.UI.CLI.Interfaces;
using System;

namespace Chess.UI.CLI
{
    public class GameLoop
    {
        private readonly ScreenManager _screenManager;
        private readonly IInputHandler _input;

        public GameLoop(ScreenManager screenManager, IInputHandler input)
        {
            _screenManager = screenManager;
            _input = input;
        }

        public void Run()
        {
            while (!_screenManager.IsExitRequested) {
                
                _screenManager.Render();

                var action = _input.ReadAction();

                _screenManager.HandleInput(action);
            }
        }

        //public void Run()
        //{
        //    while (true) // можно добавить проверку на конец игры в конкретном экране
        //    {
        //        _screenManager.Update();
        //        System.Threading.Thread.Sleep(50); // сглаживаем мерцание
        //    }

        //    //while (!gameOver) {
        //    //    Render();

        //    //    var currentPlayer = GetCurrentPlayer();

        //    //    if (currentPlayer is HumanPlayer) {
        //    //        var action = input.ReadAction();
        //    //        controller.ProcessAction(action);
        //    //    }
        //    //    else {
        //    //        var moves = engine.GetAllMoves(position);
        //    //        var move = currentPlayer.ChooseMove(position, moves);

        //    //        controller.DoMove(move);

        //    //        Thread.Sleep(500);
        //    //    }
        //    //}
        //}
    }
}
