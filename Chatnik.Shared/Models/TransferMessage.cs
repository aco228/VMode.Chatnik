using Chatnik.Shared.Interfaces;

namespace Chatnik.Shared.Models
{
    public class TransferMessage : ITransferMessage
    {
        public string Topic { get; set; }
        public string User { get; set; }

        public TransferMessage(string topic)
        {
            Topic = topic;
        }
    }
}