namespace Chess.UI.CLI.Models
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
        NewGame,       // новая игра
        Escape,         // выход в меню
        PageUp,       //прокрутка какой то текстовой информации
        PageDown,
    }

    public record PlayerAction(PlayerActionType Type);
}
