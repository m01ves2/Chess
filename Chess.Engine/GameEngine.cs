using Chess.Domain;
using Chess.Domain.Pieces;
using Chess.Engine.Results;
using System.ComponentModel;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

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
        private IEnumerable<Position> GeneratePseudoMoves(Position pos)
        {
            var piece = _board.GetSquare(pos).Piece;

            if (piece == null)
                return Enumerable.Empty<Position>();


            return piece switch
            {
                Pawn => GeneratePawnMoves(pos, piece),
                _ => GenerateNotPawnMoves(pos, piece)
            };

        }
     
        private IEnumerable<Position> GeneratePawnMoves(Position pos, Piece pawn)
        {
            foreach (var offset in pawn.GetMoveOffsets()) {
                switch (offset.MoveType) {
                    case MoveType.Normal:
                        foreach (var move in TryAddNormalPawnMove(pos, pawn, offset)) yield return move;
                        break;
                    case MoveType.PawnFirstMove:
                        foreach (var move in TryAddPawnFirstMove(pos, pawn, offset)) yield return move;
                        break;
                    case MoveType.PawnAttack:
                        foreach (var move in TryAddPawnAttackMove(pos, pawn, offset)) yield return move;
                        break;
                    case MoveType.PawnEnPassant:
                        foreach (var move in TryAddEnPassantMove(pos, pawn, offset)) yield return move;
                        break;
                }
            }
        }

        private IEnumerable<Position> TryAddNormalPawnMove(Position pos, Piece pawn, MoveOffset offset)
        {
            var target = pos + offset * offset.MaxDistance;
            if (!_board.IsInsideBoard(target))
                yield break;

            if (_board.GetSquare(target).IsEmpty()) {
                yield return target;
            }
        }

        private IEnumerable<Position> TryAddPawnFirstMove(Position pos, Piece pawn, MoveOffset offset)
        {
            if (!IsPawnOnStartPosition(pos, pawn))
                yield break;

            var oneStep = pos + offset;
            var twoSteps = oneStep + offset;

            if (!_board.IsInsideBoard(twoSteps))
                yield break;

            if (_board.GetSquare(oneStep).IsEmpty() &&
                _board.GetSquare(twoSteps).IsEmpty()) {
                yield return twoSteps;
            }
        }

        bool IsPawnOnStartPosition(Position pos, Piece piece)
        {
            if (piece.Color == Domain.PieceColor.White && pos.Row == 6)
                return true;
            if (piece.Color == Domain.PieceColor.Black && pos.Row == 1)
                return true;
            return false;
        }

        private IEnumerable<Position> TryAddPawnAttackMove(Position pos, Piece pawn, MoveOffset offset)
        {
            var target = pos + offset;
            if (!_board.IsInsideBoard(target))
                yield break;

            var targetSquare = _board.GetSquare(target);
            if (targetSquare.Piece != null && pawn.Color != targetSquare.Piece.Color)
               yield return target;
        }
        private IEnumerable<Position> TryAddEnPassantMove(Position pos, Piece pawn, MoveOffset offset)
        {
            //TODO
            return Enumerable.Empty<Position>();
        }

        


        // маленькие методы для каждого типа хода
        private IEnumerable<Position> GenerateNotPawnMoves(Position pos, Piece piece)
        {
            var possibleMoves = new List<Position>();
            foreach (var offset in piece.GetMoveOffsets()) {
                for (int step = 1; step <= offset.MaxDistance; step++) {
                    var target = pos + offset * step;
                    if (!_board.IsInsideBoard(target)) 
                        break;

                    if (offset.MoveType == MoveType.Normal) {
                        var targetSquare = _board.GetSquare(target);
                        if (targetSquare.IsEmpty()) {
                            possibleMoves.Add(target);
                        }
                        else {
                            if (targetSquare.Piece!.Color != piece.Color) {
                                possibleMoves.Add(target);
                            }
                            break;
                        }
                    }
                    else {
                        switch (offset.MoveType) {
                            case MoveType.KingCastling:
                                foreach (var move in TryAddKingCastlingMove(pos, piece, offset)) possibleMoves.Add(move);
                                break;
                            case MoveType.KingLongCastling:
                                foreach (var move in TryAddKingLongCastlingMove(pos, piece, offset)) possibleMoves.Add(move);
                                break;

                            //возможно, еще какие то специальные ходы
                        }
                    }                
                }
            }
            return possibleMoves;
        }

        private IEnumerable<Position> TryAddKingCastlingMove(Position pos, Piece king, MoveOffset offset)
        {
            if (!CanKingCastling(king))
                yield break;

            var delta = new MoveOffset(1, 0, 1);
            var oneStep = pos + delta;
            var twoSteps = oneStep + delta;

            if (_board.GetSquare(oneStep).IsEmpty() &&
                _board.GetSquare(twoSteps).IsEmpty()) {
                yield return twoSteps;
            }
        }

        private bool CanKingCastling(Piece king)
        {
            if(king.Color == PieceColor.White && !State.WhiteKingMoved && !State.WhiteRookH_Moved)
                return true;
            if (king.Color == PieceColor.Black && !State.BlackKingMoved && !State.BlackRookH_Moved)
                return true;
            return false;
        }

        private IEnumerable<Position> TryAddKingLongCastlingMove(Position pos, Piece king, MoveOffset offset)
        {
            if (!CanKingCastling(king))
                yield break;

            var delta = new MoveOffset(-1, 0, 1);
            var oneStep = pos + delta;
            var twoSteps = oneStep + delta;
            var threeSteps = twoSteps + delta;

            if (_board.GetSquare(oneStep).IsEmpty() &&
                _board.GetSquare(twoSteps).IsEmpty() &&
                _board.GetSquare(threeSteps).IsEmpty()) {
                yield return twoSteps;
            }
        }

        private bool CanKingLongCastling(Piece king)
        {
            if (king.Color == PieceColor.White && !State.WhiteKingMoved && !State.WhiteRookA_Moved)
                return true;
            if (king.Color == PieceColor.Black && !State.BlackKingMoved && !State.BlackRookA_Moved)
                return true;
            return false;
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

            //TODO изменить GameState, если нужно
            //TODO специальные ходы. такие как "обращение пешки"
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
