using Chess.Domain;

namespace Chess.Application
{
    public enum PlayerActionType
    {
        None,
        MoveUp,       // игрок перемещает курсор по клеткам
        MoveDown,
        MoveLeft,
        MoveRight,
        Select,       // игрок выбирает клетку с фигурой или клетку назначения
        Undo,         // отмена хода - через Esc либо клик по уже выбранной клетке
        Promotion,
        Quit,         // выход
        NewGame       // новая игра
    }

    public record PlayerAction(PlayerActionType Type);
}
