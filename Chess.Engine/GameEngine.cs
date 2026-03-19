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

        public IEnumerable<Move> GetLegalMoves(Position currentPos)
        {
            //TODO обработка срубания
            var pseudoMoves = GeneratePseudoMoves(currentPos);
            //simulate move
            //IsKingInCheck

            //GetLegalMoves
            //foreach move in GetPseudoMoves
            //    simulate
            //if !kingInCheck
            //    add



            //var pseudoMovies = GeneratePseudoMoves(currentPos);
            //var piece = _board.GetSquare(currentPos).Piece;
            //foreach (var move in pseudoMovies) {
            //    if(!IsKingInCheck(piece.Color))
            //        yield return move;
            //}
            return pseudoMoves;
        }

        public MoveResult TryMove(Move move)
        {
            var piece = move.Piece;
            if (piece == null)
                return new MoveResult(ResultStatus.Invalid);

            var legalMoves = GetLegalMoves(move.From).ToList(); //TODO
            if (!legalMoves.Contains(move))
                return new MoveResult(ResultStatus.Invalid);

            return new MoveResult(ResultStatus.Success, move.CapturedPiece);
        }

        private IEnumerable<Move> GeneratePseudoMoves(Position pos)
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
     
        //методы для хода пешки
        private IEnumerable<Move> GeneratePawnMoves(Position pos, Piece pawn)
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
        private IEnumerable<Move> TryAddNormalPawnMove(Position pos, Piece pawn, MoveOffset offset)
        {
            var oneStep = pos + offset * offset.MaxDistance;
            if (!_board.IsInsideBoard(oneStep))
                yield break;

            if (_board.GetSquare(oneStep).IsEmpty()) {
                yield return new Move(pos, oneStep, pawn);
            }
        }
        private IEnumerable<Move> TryAddPawnDoubleMove(Position pos, Piece pawn, MoveOffset offset)
        {
            if (!IsPawnOnStartPosition(pos, pawn))
                yield break;

            var oneStep = pos + offset;
            var twoSteps = oneStep + offset;

            if (!_board.IsInsideBoard(twoSteps))
                yield break;

            if (_board.GetSquare(oneStep).IsEmpty() &&
                _board.GetSquare(twoSteps).IsEmpty()) {
                yield return new Move(pos, twoSteps, pawn);
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
        private IEnumerable<Move> TryAddPawnAttackMove(Position pos, Piece pawn, MoveOffset offset)
        {
            var target = pos + offset; //атака по диагонали
            if (!_board.IsInsideBoard(target))
                yield break;

            var targetSquare = _board.GetSquare(target);

            if (targetSquare.Piece == null) //некого атаковать
                yield break;

            if (targetSquare.Piece.Color == pawn.Color) //нельзя атаковать своих
                yield break;

            yield return new Move(pos, target, pawn, targetSquare.Piece);
        }
        private IEnumerable<Move> TryAddEnPassantMove(Position pos, Piece pawn, MoveOffset offset)
        {
            //TODO
            return Enumerable.Empty<Move>();
        }


        //  методы для хода не пешки
        private IEnumerable<Move> GenerateNotPawnMoves(Position pos, Piece piece)
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
        private IEnumerable<Move> GenerateSlidingMoves(Position pos, Piece piece, MoveOffset offset)
        {
            for (int step = 1; step <= offset.MaxDistance; step++) {
                var target = pos + offset * step;

                if (!_board.IsInsideBoard(target))
                    yield break;

                var targetSquare = _board.GetSquare(target);

                if (targetSquare.IsEmpty()) {
                    yield return new Move(pos, target, piece);

                    continue;
                }

                if (targetSquare.Piece.Color != piece.Color) {
                    yield return new Move(pos, target, piece, targetSquare.Piece);
                }

                yield break;
            }
        }
        private IEnumerable<Move> GenerateSpecialMove(Position pos, Piece piece, MoveOffset offset)
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
        private IEnumerable<Move> TryAddKingCastlingMove(Position pos, Piece king, MoveOffset offset)
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

            yield return new Move(pos, twoSteps, king); //TODO
        }
        private bool CanKingCastling(Piece king)
        {
            if(king.Color == PieceColor.White && !_state.WhiteKingMoved && !_state.WhiteRookH_Moved)
                return true;
            if (king.Color == PieceColor.Black && !_state.BlackKingMoved && !_state.BlackRookH_Moved)
                return true;
            return false;
        }
        private IEnumerable<Move> TryAddKingLongCastlingMove(Position pos, Piece king, MoveOffset offset)
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
                yield return new Move(pos, twoSteps, king); //TODO
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

        public bool IsKingInCheck(PieceColor color)
        {
            var kingSquare = FindKingSquare(color);

            foreach (var square in _board.Squares) {
                var piece = square.Piece;
                if (piece == null || piece.Color == color) continue;

                var positions = GeneratePseudoMoves(square.Position);
                
                if( positions.Contains(kingSquare.Position))
                    return true;
            }

            return false;
        }

        public Square? FindKingSquare(PieceColor color)
        {
            foreach (var square in _board.Squares)
                if (square.Piece != null && square.Piece is King && square.Piece.Color == color)
                    return square;
            return null;
        }

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
