using Chess.Domain;

namespace Chess.Engine.Results
{
    public enum ResultStatus
    {
        Success, 
        Fail,
        Invalid,
    }
    
    public class MoveResult
    {
        public ResultStatus Status { get; set; }
        public Piece? CapturedPiece { get; set; }

        public MoveResult(ResultStatus status, Piece? capturedPiece = null)
        {
            Status = status;
            CapturedPiece = capturedPiece;
        }
    }
}
