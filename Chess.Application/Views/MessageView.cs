namespace Chess.Application.ViewModels
{
    public enum MessageType
    {
       None,
       Promotion,
    }
    public class MessageView
    {
        public MessageType Type;
        public string? Text;
    }
}
