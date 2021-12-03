namespace Chatnik.Shared.Models
{
    public enum ChatMessageType
    {
        Message,
        UserJoined,
        UserLeft,
        ServerLostConnection,
        ServerRegainedConnection,
    }
}