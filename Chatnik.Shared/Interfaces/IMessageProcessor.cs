namespace Chatnik.Shared.Interfaces
{
    public interface IMessageProcessor
    {
        public void ProcessMessage(IReceiveMessage message);
    }
}