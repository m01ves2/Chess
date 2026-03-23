    using Chess.Domain;
using Chess.Domain.Moves;
using Chess.Domain.Pieces;

namespace Chess.Engine
{
    public class GameEngine
    {
        //public void UpdateBoard(Board board)
        //{
        //    _board = board;
        //}

        //public GameState GetState() => _state;

        public IEnumerable<Move> GetLegalMoves(GamePosition gamePosition, Position from)
        {
            var pseudoMoves = GeneratePseudoMoves(gamePosition, from);
            foreach (var move in pseudoMoves) {
                if (!WouldBeCheck(gamePosition, move)) {
                    yield return move;
                    continue;
                }
            }
            //return pseudoMoves;
        }

        private IEnumerable<Move> GeneratePseudoMoves(GamePosition gamePosition, Position pos)
        {
            var piece = gamePosition.Board.GetSquare(pos).Piece;

            if (piece == null)
                yield break;

            if (piece is Pawn) {
                foreach (var move in AddPawnMoves(gamePosition, pos, piece))
                    yield return move;
            }
            else {
                foreach (var move in AddNotPawnMoves(gamePosition, pos, piece))
                    yield return move;
            }

        }
     
        //методы для хода пешки
        private IEnumerable<Move> AddPawnMoves(GamePosition gamePosition, Position pos, Piece pawn)
        {
            foreach (var offset in pawn.GetMoveOffsets()) {
                switch (offset.MoveType) {
                    case MoveType.Normal:
                        foreach (var move in AddNormalPawnMove(gamePosition, pos, pawn, offset)) yield return move;
                        break;
                    case MoveType.PawnFirstMove:
                        foreach (var move in AddPawnDoubleMove(gamePosition, pos, pawn, offset)) yield return move;
                        break;
                    case MoveType.PawnAttack:
                        foreach (var move in AddPawnAttackMove(gamePosition, pos, pawn, offset)) yield return move;
                        break;
                    case MoveType.PawnEnPassant:
                        foreach (var move in AddEnPassantMove(gamePosition, pos, pawn, offset)) yield return move;
                        break;
                }
            }
        }
        private IEnumerable<Move> AddNormalPawnMove(GamePosition gamePosition, Position pos, Piece pawn, MoveOffset offset)
        {
            var oneStep = pos + offset * offset.MaxDistance;
            if (!gamePosition.Board.IsInsideBoard(oneStep))
                yield break;


            if (gamePosition.Board.GetSquare(oneStep).IsEmpty()) {
                if ((oneStep.Row == 0 && pawn.Color == PieceColor.White) ||
                (oneStep.Row == 7 && pawn.Color == PieceColor.Black)) {
                    yield return new PromotionMove(pos, oneStep, pawn, true);
                }
                else {    
                    yield return new NormalMove(pos, oneStep, pawn);
                }
            }
        }
        private IEnumerable<Move> AddPawnDoubleMove(GamePosition gamePosition, Position pos, Piece pawn, MoveOffset offset)
        {
            if (!IsPawnOnStartPosition(pos, pawn))
                yield break;

            var oneStep = pos + offset;
            var twoSteps = oneStep + offset;

            if (!gamePosition.Board.IsInsideBoard(twoSteps))
                yield break;

            if (gamePosition.Board.GetSquare(oneStep).IsEmpty() &&
                gamePosition.Board.GetSquare(twoSteps).IsEmpty()) {
                yield return new PawnDoubleMove(pos, twoSteps, pawn, oneStep);
            }
        }
        bool IsPawnOnStartPosition(Position pos, Piece piece)
        {
            if (piece.Color == PieceColor.White && pos.Row == 6)
                return true;
            if (piece.Color == PieceColor.Black && pos.Row == 1)
                return true;
            return false;
        }
        private IEnumerable<Move> AddPawnAttackMove(GamePosition gamePosition, Position pos, Piece pawn, MoveOffset offset)
        {
            var target = pos + offset; //атака по диагонали
            if (!gamePosition.Board.IsInsideBoard(target))
                yield break;

            var targetSquare = gamePosition.Board.GetSquare(target);

            if (targetSquare.Piece == null) //некого атаковать
                yield break;

            if (targetSquare.Piece.Color == pawn.Color) //нельзя атаковать своих
                yield break;

            yield return new NormalMove(pos, target, pawn, targetSquare.Piece);
        }
        private IEnumerable<Move> AddEnPassantMove(GamePosition gamePosition, Position pos, Piece pawn, MoveOffset offset)
        {
            if(gamePosition.State.EnPassantTarget == null) 
                yield break; //некого атаковать

            var enPassantTarget = gamePosition.State.EnPassantTarget;
            var enPassantTargetSquare = gamePosition.Board.GetSquare( enPassantTarget.Value);
            var step = pos + offset;
            if (enPassantTarget == step) {
                var capturedPiecePosition = new Position(pos.Row, step.Col);
                var capturedPiece = gamePosition.Board.GetSquare(capturedPiecePosition).Piece!;
                yield return new EnPassantMove(pos, step, pawn, capturedPiecePosition, capturedPiece);
            }
            
            yield break;
        }

        //public void AddPromotePawn(Piece piece, Position pos)
        //{
        //    if (piece is not Pawn pawn) 
        //        return;

        //    if (pawn.Color == PieceColor.White && pos.Row == 0)
        //        piece = new Queen(PieceColor.White);
        //    else if(pawn.Color == PieceColor.Black && pos.Row == 7)
        //        piece = new Queen(PieceColor.Black);

        //}


        //  методы для хода не пешки
        private IEnumerable<Move> AddNotPawnMoves(GamePosition gamePosition, Position pos, Piece piece)
        {
            foreach (var offset in piece.GetMoveOffsets()) {
                if (offset.MoveType == MoveType.Normal) {
                    foreach (var move in AddNotPawnNormalMoves(gamePosition, pos, piece, offset))
                        yield return move;

                    continue;
                }

                foreach (var move in AddNotPawnSpecialMove(gamePosition, pos, piece, offset))
                    yield return move;
            }
        }
        private IEnumerable<Move> AddNotPawnNormalMoves(GamePosition gamePosition, Position pos, Piece piece, MoveOffset offset)
        {
            for (int step = 1; step <= offset.MaxDistance; step++) {
                var target = pos + offset * step;

                if (!gamePosition.Board.IsInsideBoard(target))
                    yield break;

                var targetSquare = gamePosition.Board.GetSquare(target);

                if (targetSquare.IsEmpty()) {
                    yield return new NormalMove(pos, target, piece);

                    continue;
                }

                if (targetSquare.Piece!.Color != piece.Color) {
                    yield return new NormalMove(pos, target, piece, targetSquare.Piece);
                }

                yield break;
            }
        }
        private IEnumerable<Move> AddNotPawnSpecialMove(GamePosition gamePosition, Position pos, Piece piece, MoveOffset offset)
        {
            switch (offset.MoveType) {
                case MoveType.KingCastling:
                    foreach (var move in AddCastlingMove(gamePosition, pos, piece, offset))
                        yield return move;
                    break;
                case MoveType.KingLongCastling:
                    foreach (var move in AddLongCastlingMove(gamePosition, pos, piece, offset))
                        yield return move;
                    break;
                    // другие спец. ходы
            }
        }
        private IEnumerable<Move> AddCastlingMove(GamePosition gamePosition, Position pos, Piece king, MoveOffset offset)
        {
            if (!CanCastling(gamePosition, king))
                yield break;

            var oneStep = new Position(pos.Row, pos.Col + 1); // e1 -> f1
            var twoSteps = new Position(pos.Row, pos.Col + 2); // e1 -> g1

            if (!gamePosition.Board.GetSquare(oneStep).IsEmpty())
                yield break;

            if (!gamePosition.Board.GetSquare(twoSteps).IsEmpty())
                yield break;

            yield return new KingCastlingMove(pos, twoSteps, king, true);
        }
        private bool CanCastling(GamePosition gamePosition, Piece king)
        {
            if(king.Color == PieceColor.White && !gamePosition.State.WhiteKingMoved && !gamePosition.State.WhiteRookH_Moved)
                return true;
            if (king.Color == PieceColor.Black && !gamePosition.State.BlackKingMoved && !gamePosition.State.BlackRookH_Moved)
                return true;
            return false;
        }
        private IEnumerable<Move> AddLongCastlingMove(GamePosition gamePosition, Position pos, Piece king, MoveOffset offset)
        {
            if (!CanLongCastling(gamePosition, king))
                yield break;

            var delta = new MoveOffset(1, 0, 1);

            var oneStep = new Position(pos.Row, pos.Col - 1); // e1 -> f1
            var twoSteps = new Position(pos.Row, pos.Col - 2); // e1 -> g1
            var threeSteps = new Position(pos.Row, pos.Col - 3); // e1 -> g1


            if (!gamePosition.Board.GetSquare(oneStep).IsEmpty())
                yield break;

            if (!gamePosition.Board.GetSquare(twoSteps).IsEmpty())
                yield break;

            if (!gamePosition.Board.GetSquare(threeSteps).IsEmpty())
                yield break;

            yield return new KingCastlingMove(pos, twoSteps, king, false);

        }

        private bool CanLongCastling(GamePosition gamePosition, Piece king)
        {
            if (king.Color == PieceColor.White && !gamePosition.State.WhiteKingMoved && !gamePosition.State.WhiteRookA_Moved)
                return true;
            if (king.Color == PieceColor.Black && !gamePosition.State.BlackKingMoved && !gamePosition.State.BlackRookA_Moved)
                return true;
            return false;
        }


        public void MakeMove(GamePosition gamePosition, Move move)
        {
            gamePosition.State.EnPassantTarget = null;
            move.Apply(gamePosition);
            UpdateGameState(gamePosition, move);
        }

        public void UpdateGameState(GamePosition gamePosition, Move move)
        {
            if (move.Piece is King && move.Piece.Color == PieceColor.White) {
                gamePosition.State.WhiteKingMoved = true;
            }
            else if (move.Piece is King && move.Piece.Color == PieceColor.Black) {
                gamePosition.State.BlackKingMoved = true;
            }
            else if (move.Piece is Rook && move.Piece.Color == PieceColor.White) {
                if (move.From.Col == 0)
                    gamePosition.State.WhiteRookA_Moved = true;
                if (move.From.Col == 7)
                    gamePosition.State.WhiteRookH_Moved = true;
            }
            else if (move.Piece is Rook && move.Piece.Color == PieceColor.Black) {
                if (move.From.Col == 0)
                    gamePosition.State.BlackRookA_Moved = true;
                if (move.From.Col == 7)
                    gamePosition.State.BlackRookH_Moved = true;
            }
        }

        public bool IsKingInCheck(GamePosition gamePosition, PieceColor color)
        {
            var kingSquare = FindKingSquare(gamePosition, color);

            foreach (var square in gamePosition.Board.Squares) {
                var piece = square.Piece;
                if (piece == null || piece.Color == color) continue;

                var moves = GeneratePseudoMoves(gamePosition, square.Position).ToList();

                if (moves.Any(m => m.To == kingSquare!.Position))
                    return true;
            }

            return false;
        }

        public Square? FindKingSquare(GamePosition gamePosition, PieceColor color)
        {
            foreach (var square in gamePosition.Board.Squares)
                if (square.Piece != null && square.Piece is King && square.Piece.Color == color)
                     return square;
            return null;
        }

        public bool WouldBeCheck(GamePosition gamePosition, Move move)
        {
            //var testBoard = _board.Clone();
            //var testState = _state.Clone();
            //var testEngine = new GameEngine(testBoard, testState);
            var testGamePosition = gamePosition.Clone();
            var testEngine = new GameEngine();
            
            testEngine.MakeMove(testGamePosition, move);
            return testEngine.IsKingInCheck(testGamePosition, move.Piece.Color);
        }
    }
}
