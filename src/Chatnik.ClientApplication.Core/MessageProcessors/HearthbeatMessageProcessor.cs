using Chatnik.Shared.Implementations;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;
using Microsoft.Extensions.Logging;

namespace Chatnik.ClientApplication.Core.MessageProcessors
{
    public interface IHearthbeatMessageProcessor : IMessageProcessor
    {
        public event OnMessageReceived MessageReceived;

        public delegate void OnMessageReceived();
    }
    
    public class HearthbeatMessageProcessor : MessageProcessorBase<TransferMessage>, IHearthbeatMessageProcessor
    {
        private readonly ILogger<HearthbeatMessageProcessor> _logger;
        
        public HearthbeatMessageProcessor(ILogger<HearthbeatMessageProcessor> logger)
        {
            _logger = logger;
        }
        
        protected override void Process(TransferMessage message)
        {
            _logger.LogInformation($"HB RECEIVED {message.Topic}");
            MessageReceived?.Invoke();
        }

        public event IHearthbeatMessageProcessor.OnMessageReceived? MessageReceived;
    }
}