using System;
using System.Linq;
using Chatnik.Shared;
using Chatnik.Shared.Implementations;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;
using Microsoft.Extensions.Logging;

namespace Chatnik.ServerApplication.MessageProcessors
{
    public class ReceiveAndReturnMessageProcessor : MessageProcessorBase<TransferMessage>, IReceiveAndReturnMessageProcessor
    {
        private readonly ILogger<ReceiveAndReturnMessageProcessor> _logger;
        private readonly IChatnikPublisherSocket _publisher;
        
        public ReceiveAndReturnMessageProcessor(
            ILogger<ReceiveAndReturnMessageProcessor> logger,
            IChatnikPublisherSocket publisher)
        {
            _logger = logger;
            _publisher = publisher;
        }
        
        protected override void Process(TransferMessage message)
        {
            // Actuall processing will be done in ProcessMessage
            // as we dont need derivate, but actuall raw message to
            // transmit to all subscribers
            throw new NotImplementedException();
        }

        public override void ProcessMessage(IReceiveMessage message)
        {
            //_logger.LogInformation($"Received topic {message.Topic} from {message.User}");

            // TODO! Little hack, to show only chat messages
            if (message.Topic == GlobalConstants.MainChannel)
            {
                _logger.LogInformation($"{message.User} : {string.Join(", ", message.Frames.ToArray())}");
            }
            
            _publisher.Send(message.NetMQMessage);
        }
    }
}