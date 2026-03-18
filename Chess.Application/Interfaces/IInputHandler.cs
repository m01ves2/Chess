using Chess.Domain;
using System;
using System.Diagnostics.Metrics;

namespace Chess.Application.Interfaces
{
//    Основная идея: обрабатывает только ввод пользователя, не думая о логике игры.

//В CLI это может быть:

//Стрелки → перемещают курсор

//Enter / Space → выбрать фигуру или цель хода

//В GUI это может быть: клик мышью или касание экрана

//Output: действие игрока в виде структуры PlayerAction (например, Select(Position) или Move(Position from, Position to)), не Board, не Game
    public interface IInputHandler
    {
        PlayerAction ReadAction();
    }
}
