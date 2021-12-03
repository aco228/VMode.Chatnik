using NetMQ;

namespace Chatnik.Shared.Interfaces
{
    public interface IChatnikPublisherSocket : IChatnikSocket
    {
        public string CurrentUser { get; }
        public void Send(ITransferMessage message);
        public void Send(NetMQMessage message);
    }
}