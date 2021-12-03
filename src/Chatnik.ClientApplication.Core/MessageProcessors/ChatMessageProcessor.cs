using Chatnik.Shared.Implementations;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;

namespace Chatnik.ClientApplication.Core.MessageProcessors
{
    public interface IChatMessageProcessor : IMessageProcessor
    {
        public event OnMessageReceived MessageReceived;

        public delegate void OnMessageReceived(ChatMessage message);
    }
    
    public class ChatMessageProcessor : MessageProcessorBase<ChatMessage>, IChatMessageProcessor
    {
        protected override void Process(ChatMessage message)
        {
            MessageReceived?.Invoke(message);
        }

        public event IChatMessageProcessor.OnMessageReceived? MessageReceived;
    }
}