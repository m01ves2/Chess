using Chess.Application;
using Chess.Application.Interfaces;
using Chess.Domain;
using Chess.Domain.Moves;
using Chess.Domain.Pieces;
using System.Security.Cryptography;

namespace Chess.UI.CLI
{
    //Engine: координаты фиксированы, белые снизу, черные сверху
    //UI: CurrentPlayer == белый -> без поворота
    //UI: CurrentPlayer == черный -> поворот 180°
    //    Принцип:
    //    Без поворота(белые снизу) :

    //  a b c d e f g h   ← файлы
    //8 ♜ ♞ ♝ ♛ ♚ ♝ ♞ ♜
    //7 ♟ ♟ ♟ ♟ ♟ ♟ ♟ ♟
    //6 . . . . . . . .
    //...
    //1 ♖ ♘ ♗ ♕ ♔ ♗ ♘ ♖
    //^
    //ранги

    //С поворотом на 180° (черные снизу):

    //  h g f e d c b a   ← файлы «зеркально»  
    //1 ♖ ♘ ♗ ♕ ♔ ♗ ♘ ♖
    //2 ♙ ♙ ♙ ♙ ♙ ♙ ♙ ♙
    //...
    //8 ♜ ♞ ♝ ♛ ♚ ♝ ♞ ♜
    //^
    //ранги «перевернуты»

    //еще по поводу поворотов:
    //    Human vs AI: доска статична, Human всегда снизу
    //Human vs Human: UI поворачивает доску так, чтобы текущий игрок был снизу
    //AI vs AI: UI может просто показывать доску в фиксированном положении(или отключить визуализацию)
    //UI поворачивает отображение:
    //rotatedRow = 7 - row;
    //rotatedCol = 7 - col;
    public class CLIBoardRenderer : IBoardRenderer
    {
        public void Render(GameScene gameScene)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;

            RenderFiles();
            RenderBoard(gameScene);
            RenderRanks();

            RenderKingInCheckMessage(gameScene);
            RenderMoveHistory(gameScene.MoveHistory);
        }

        public void RenderFiles()
        {
            int CellWidth = 3;

            // Верхняя нумерация файлов (a-h)
            int d = 1;
            for (char f = 'a'; f <= 'h'; f++) {
                Console.SetCursorPosition(CellWidth * d, 0);
                d++;
                Console.Write(f + " ");
            }
        }

        public void RenderRanks()
        {
            int YOffset = 2;
            int CellHeight = 3;

            // Левый столбец с рангами (1-8)
            for (int r = 0; r < Board.BoardSize; r++) {
                Console.SetCursorPosition(0, r * CellHeight + YOffset);
                Console.Write(8 - r);
            }
        }

        private void RenderBoard(GameScene game)
        {
            for (int row = 0; row < Board.BoardSize; row++) {
                for (int col = 0; col < Board.BoardSize; col++) {
                    var pos = new Position(row, col);
                    RenderCell(game, pos);
                }
            }
        }

        private void RenderCell(GameScene game, Position pos)
        {
            var square = game.Board.GetSquare(pos);

            bool isCursor = game.Cursor == pos;
            bool isSelected = game.SelectedPosition == pos;
            bool isHighlighted = game.HighlightedPositions.Contains(pos);

            ConsoleColor bg = GetBackgroundColor(pos, isSelected, isHighlighted);
            ConsoleColor fg = GetPieceColor(square) ?? ConsoleColor.Yellow;

            char piece = GetPieceSymbol(square);

            DrawCell(pos, piece, fg, bg, isCursor);
        }

        private ConsoleColor GetBackgroundColor(Position pos, bool selected, bool highlighted)
        {
            if (selected)
                return ConsoleColor.DarkGreen;
            if (highlighted)
                return ConsoleColor.DarkYellow;

            return (pos.Row + pos.Col) % 2 == 0 ? ConsoleColor.Gray : ConsoleColor.DarkGray;
        }

        private ConsoleColor? GetPieceColor(Square square)
        {
            if (square.IsEmpty())
                return null;
            else if (square.Piece!.Color == PieceColor.Black)
                return ConsoleColor.Black;
            else
                return ConsoleColor.White;
        }

        private char GetPieceSymbol(Square square)
        {
            return square.Piece == null ? ' ' : GetSymbol(square.Piece);
        }

        private void DrawCell(Position pos, char piece, ConsoleColor fg, ConsoleColor bg, bool isCursor)
        {
            int XOffset = 2;
            int YOffset = 1;
            int CellWidth = 3;
            int CellHeight = 3;

            int x = XOffset + pos.Col * CellWidth;
            int y = YOffset + pos.Row * CellHeight;

            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;

            string top = isCursor ? "┌─┐" : "   ";
            string middle = isCursor ? $"│{piece}│" : $" {piece} ";
            string bottom = isCursor ? "└─┘" : "   ";

            Console.SetCursorPosition(x, y);
            Console.Write(top);
            Console.SetCursorPosition(x, y + 1);
            Console.Write(middle);
            Console.SetCursorPosition(x, y + 2);
            Console.Write(bottom);
            Console.ResetColor();
        }

        public static char GetSymbol(Piece piece) => piece switch
        {
            Rook r when r.Color == PieceColor.White => '\u2656',
            Rook r => '\u265C',
            Knight n when n.Color == PieceColor.White => '\u2658',
            Knight n => '\u265E',
            Bishop b when b.Color == PieceColor.White => '\u2657',
            Bishop b => '\u265D',
            Queen q when q.Color == PieceColor.White => '\u2655',
            Queen q => '\u265B',
            King k when k.Color == PieceColor.White => '\u2654',
            King k => '\u265A',
            Pawn p when p.Color == PieceColor.White => '\u2659',
            Pawn p => '\u265F',
            _ => '?'
        };

        private Position Rotate180(Position pos)
        {
            return new Position(7 - pos.Row, 7 - pos.Col);
        }

        private void RenderKingInCheckMessage(GameScene game)
        {
            Console.SetCursorPosition(40, 3);
            if (game.BlackKingInCheck) {
                Console.WriteLine("Black King is in check!");
            }
            else if (game.WhiteKingInCheck) {
                Console.WriteLine("White King is in check!");
            }
            else {
                Console.WriteLine("                       "); //TODO
            }
        }

        private void RenderMoveHistory(List<Move> history)
        {
            Console.SetCursorPosition(40, 5);
            Console.WriteLine("Move history: ");
            for (int i = 0; i < history.Count; i++) {
                Console.SetCursorPosition(40, 6 + i);
                Console.Write($"#{i + 1}.{NotationMapper.PositionToString(history[i].From)} - {NotationMapper.PositionToString(history[i].To)}");

                if (history[i] is NormalMove nm) {
                    Console.WriteLine(nm.CapturedPiece == null ? " " : $" (captured: {nm.CapturedPiece}) ");
                }
                else if (history[i] is EnPassantMove ep) {
                    Console.WriteLine( $" (captured: {ep.CapturedPiece}) ");
                }
            }
        }
    }
}
