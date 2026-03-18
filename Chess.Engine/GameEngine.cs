using Chess.Domain;
using Chess.Domain.Pieces;
using Chess.Engine.Results;
using System.ComponentModel;
using System.Drawing;

namespace Chess.Engine
{
    public class GameEngine
    {
        private Board _board;
        public GameState State = new GameState();

        public GameEngine(Board board)
        {
            _board = board;
        }

        public void UpdateBoard(Board board)
        {
            _board = board;
        }


        //public GameState GetState() => _state;

        // Возвращает допустимые ходы фигуры с позиции currentPos
        public IEnumerable<Position> GeneratePseudoMoves(Position currentPos)
        {
            var piece = _board.GetSquare(currentPos).Piece;

            if (piece == null)
                return Enumerable.Empty<Position>();

            if (piece is Pawn)
                return GeneratePawnPseudoMoves(currentPos);
            else
                return GenerateExceptPawnPseudoMoves(currentPos);
        }

        public IEnumerable<Position> GeneratePawnPseudoMoves(Position currentPos)
        {
            var piece = _board.GetSquare(currentPos).Piece;
            var moveOffsets = piece!.GetMoveOffsets();

            var possibleMoves = new List<Position>();
            foreach (var move in moveOffsets) {
                for (var step = 1; step <= move.MaxDistance; step++) {
                    var possibleMove = currentPos + move * step;

                    if (!_board.IsInsideBoard(possibleMove))
                        break;

                    var possibleSquare = _board.GetSquare(possibleMove);

                    if (possibleSquare.IsEmpty()) {
                        possibleMoves.Add(possibleMove);
                    }

                    var diagPos1 = new Position(possibleMove.Row, possibleMove.Col - 1);
                    var diagPos2 = new Position(possibleMove.Row, possibleMove.Col + 1);

                    if (_board.IsInsideBoard(diagPos1) && step == 1) {
                        var diagSquare1 = _board.GetSquare(diagPos1);
                        if (diagSquare1.Piece != null && piece.Color != diagSquare1.Piece.Color)
                            possibleMoves.Add(diagPos1);
                    }
                    if (_board.IsInsideBoard(diagPos2) && step == 1) {
                        var diagSquare2 = _board.GetSquare(diagPos2);
                        if (diagSquare2.Piece != null && piece.Color != diagSquare2.Piece.Color)
                            possibleMoves.Add(diagPos2);
                    }

                    if (!IsPawnOnStartPosition(piece, currentPos))
                        break;
                }
            }
            return possibleMoves;
        }

        bool IsPawnOnStartPosition(Piece piece, Position pos)
        {
            if (piece.Color == Domain.PieceColor.White && pos.Row == 6)
                return true;
            if (piece.Color == Domain.PieceColor.Black && pos.Row == 1)
                return true;
            return false;
        }

        public IEnumerable<Position> GenerateExceptPawnPseudoMoves(Position currentPos)
        {
            var piece = _board.GetSquare(currentPos).Piece;
            var moveOffsets = piece!.GetMoveOffsets();

            var possibleMoves = new List<Position>();

            foreach (var move in moveOffsets) {
                for (var step = 1; step <= move.MaxDistance; step++) {
                    var possibleMove = currentPos + move * step;

                    if (!_board.IsInsideBoard(possibleMove))
                        break;

                    var possibleSquare = _board.GetSquare(possibleMove);

                    if (possibleSquare.IsEmpty()) {
                        possibleMoves.Add(possibleMove);
                    }
                    else {
                        if (possibleSquare.Piece!.Color != piece.Color) {
                            possibleMoves.Add(possibleMove);
                        }
                        break;
                    }
                }
            }
            return possibleMoves;
        }

        public IEnumerable<Position> GetLegalMoves(Position currentPos)
        {
            //TODO обработка срубания
            return GeneratePseudoMoves(currentPos);
            //simulate move
            //IsKingInCheck

            //GetLegalMoves
            //foreach move in GetPseudoMoves
            //    simulate
            //if !kingInCheck
            //    add

        }


        public MoveResult TryMove(Position from, Position to)
        {
            var piece = _board.GetSquare(from).Piece;
            if (piece == null) 
                return new MoveResult(ResultStatus.Invalid);

            var legalMoves = GetLegalMoves(from);
            if (!legalMoves.Contains(to)) 
                return new MoveResult(ResultStatus.Invalid);

            var captured = _board.GetSquare(to).Piece;
            return new MoveResult(ResultStatus.Success, captured);
        }

        public void MakeMove(Move move)
        {
            _board.Squares[move.To.Row, move.To.Col].Piece = move.Piece;
            //move.Piece.HasMoved = true;
            _board.Squares[move.From.Row, move.From.Col].Piece = null;
        }


        //IsKingInCheck(color)
        //{
        //    kingPosition = FindKing(color)

        //    foreach enemyPiece
        //        if enemyPseudoMoves contains kingPosition
        //            return true

        //    return false
        //}

        //public bool WouldBeCheck(Move move)
        //{
        //    var snapshot = CreateSnapshot();

        //    ApplyMove(move);

        //    bool result = IsKingInCheck(_currentPlayer);

        //    Restore(snapshot);

        //    return result;
        //}
    }
}
