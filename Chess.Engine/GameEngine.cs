using Chess.Domain;
using Chess.Domain.Pieces;
using Chess.Engine.Results;

namespace Chess.Engine
{
    public class GameEngine
    {
        private Board _board;
        private GameState _state = new GameState();

        public GameEngine(Board board)
        {
            _board = board;
        }

        public void UpdateBoard(Board board)
        {
            _board = board;
        }


        //public GameState GetState() => _state;

        private IEnumerable<Position> GeneratePseudoMoves(Position pos)
        {
            var piece = _board.GetSquare(pos).Piece;

            if (piece == null)
                yield break;

            if (piece is Pawn) {
                foreach (var move in GeneratePawnMoves(pos, piece))
                    yield return move;
            }
            else {
                foreach (var move in GenerateNotPawnMoves(pos, piece))
                    yield return move;
            }

        }
     
        private IEnumerable<Position> GeneratePawnMoves(Position pos, Piece pawn)
        {
            foreach (var offset in pawn.GetMoveOffsets()) {
                switch (offset.MoveType) {
                    case MoveType.Normal:
                        foreach (var move in TryAddNormalPawnMove(pos, pawn, offset)) yield return move;
                        break;
                    case MoveType.PawnFirstMove:
                        foreach (var move in TryAddPawnDoubleMove(pos, pawn, offset)) yield return move;
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

        private IEnumerable<Position> TryAddPawnDoubleMove(Position pos, Piece pawn, MoveOffset offset)
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
            var target = pos + offset; //атака по диагонали
            if (!_board.IsInsideBoard(target))
                yield break;

            var targetSquare = _board.GetSquare(target);

            if (targetSquare.Piece == null) //некого атаковать
                yield break;

            if (targetSquare.Piece.Color == pawn.Color) //нельзя атаковать своих
                yield break;

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
            foreach (var offset in piece.GetMoveOffsets()) {
                if (offset.MoveType == MoveType.Normal) {
                    foreach (var move in GenerateSlidingMoves(pos, piece, offset))
                        yield return move;

                    continue;
                }

                foreach (var move in GenerateSpecialMove(pos, piece, offset))
                    yield return move;
            }
        }

        private IEnumerable<Position> GenerateSlidingMoves(Position pos, Piece piece, MoveOffset offset)
        {
            for (int step = 1; step <= offset.MaxDistance; step++) {
                foreach (var move in GenerateStepMove(pos, piece, offset, step))
                    yield return move;
            }
        }

        private IEnumerable<Position> GenerateStepMove(Position pos, Piece piece, MoveOffset offset, int step)
        {
            var target = pos + offset * step;
            if (!_board.IsInsideBoard(target)) yield break;

            var square = _board.GetSquare(target);
            if (square.IsEmpty()) {
                yield return target;
            }
            else if (square.Piece.Color != piece.Color) {
                yield return target;
                yield break; // дальше по этому направлению фигура идти не может!
            }
            else {
                yield break; // своя фигура — путь закрыт
            }
        }

        private IEnumerable<Position> GenerateSpecialMove(Position pos, Piece piece, MoveOffset offset)
        {
            switch (offset.MoveType) {
                case MoveType.KingCastling:
                    foreach (var move in TryAddKingCastlingMove(pos, piece, offset))
                        yield return move;
                    break;
                case MoveType.KingLongCastling:
                    foreach (var move in TryAddKingLongCastlingMove(pos, piece, offset))
                        yield return move;
                    break;
                    // другие спец. ходы
            }
        }

        private IEnumerable<Position> TryAddKingCastlingMove(Position pos, Piece king, MoveOffset offset)
        {
            if (!CanKingCastling(king))
                yield break;

            var delta = new MoveOffset(1, 0, 1);
            var oneStep = pos + delta;
            var twoSteps = oneStep + delta;

            if (!_board.GetSquare(oneStep).IsEmpty())
                yield break;

            if (!_board.GetSquare(twoSteps).IsEmpty())
                yield break;

            yield return twoSteps;
        }

        private bool CanKingCastling(Piece king)
        {
            if(king.Color == PieceColor.White && !_state.WhiteKingMoved && !_state.WhiteRookH_Moved)
                return true;
            if (king.Color == PieceColor.Black && !_state.BlackKingMoved && !_state.BlackRookH_Moved)
                return true;
            return false;
        }

        private IEnumerable<Position> TryAddKingLongCastlingMove(Position pos, Piece king, MoveOffset offset)
        {
            if (!CanKingLongCastling(king))
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
            if (king.Color == PieceColor.White && !_state.WhiteKingMoved && !_state.WhiteRookA_Moved)
                return true;
            if (king.Color == PieceColor.Black && !_state.BlackKingMoved && !_state.BlackRookA_Moved)
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

            var legalMoves = GetLegalMoves(from).ToList();
            if (!legalMoves.Contains(to))
                return new MoveResult(ResultStatus.Invalid);

            var captured = _board.GetSquare(to).Piece;
            return new MoveResult(ResultStatus.Success, captured);
        }

        public void MakeMove(Move move)
        {
            _board.Squares[move.To.Row, move.To.Col].Piece = move.Piece;
            _board.Squares[move.From.Row, move.From.Col].Piece = null;

            if (move.CapturedPiece != null) {
                if (move.CapturedPiece.Color == PieceColor.White)
                    _board.WhiteCaptured.Add(move.CapturedPiece);
                else
                    _board.BlackCaptured.Add(move.CapturedPiece);
            }

            ChangeGameState(move);

            //специальные ходы.
            //-такие как "promotion пешки"
            //TryPromotePawn(move.Piece, move.To );

            //-рокировка короля короткая

            //-рокировка короля длинная
        }

        public void ChangeGameState(Move move)
        {
            if (move.Piece is King && move.Piece.Color == PieceColor.White) {
                _state.WhiteKingMoved = true;
            }
            else if (move.Piece is King && move.Piece.Color == PieceColor.Black) {
                _state.BlackKingMoved = true;
            }
            else if (move.Piece is Rook && move.Piece.Color == PieceColor.White) {
                if(move.From.Col == 0)
                    _state.WhiteRookA_Moved = true;
                if(move.From.Col == 7)
                    _state.WhiteRookH_Moved = true;
            }
            else if (move.Piece is Rook && move.Piece.Color == PieceColor.Black) {
                if (move.From.Col == 0)
                    _state.BlackRookA_Moved = true;
                if (move.From.Col == 7)
                    _state.BlackRookH_Moved = true;
            }
        }

        //public void TryPromotePawn(Piece piece, Position pos)
        //{
        //    if (piece is not Pawn pawn) 
        //        return;

        //    if (pawn.Color == PieceColor.White && pos.Row == 0)
        //        piece = new Queen(PieceColor.White);
        //    else if(pawn.Color == PieceColor.Black && pos.Row == 7)
        //        piece = new Queen(PieceColor.Black);

        //}

        public GameState GetStateSnapshot()
        {
            // Можно вернуть глубокую копию
            return _state.Clone();
        }

        public void RestoreState(GameState snapshot)
        {
            _state = snapshot.Clone();
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
