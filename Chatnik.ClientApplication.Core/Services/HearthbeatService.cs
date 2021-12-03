using System;
using Chatnik.ClientApplication.Core.MessageProcessors;
using Chatnik.ClientApplication.Core.Models;
using Chatnik.Shared.Implementations;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;
using Microsoft.Extensions.Logging;

namespace Chatnik.ClientApplication.Core.Services
{
    public interface IHearthbeatService : IBackgroundRunner
    {
        public ServerHearthbeatStatus Status { get; }
        void StartListening(IMessageListener listener);
        
        public event OnServerNotResponding ServerNotResponding;
        public delegate void OnServerNotResponding(ServerHearthbeatStatus status);
    }
    
    public class HearthbeatService : BackgroundRunnerBase, IHearthbeatService
    {
        private readonly ILogger<HearthbeatService> _logger;
        private readonly DefaultApplicationConfiguration _configuration;
        private readonly IHearthbeatMessageProcessor _processor;
        private readonly IChatnikPublisherSocket _publisher;

        private bool _respondedInCurrentRequest = true; 
        
        public ServerHearthbeatStatus Status { get; private set; } = ServerHearthbeatStatus.Waiting; 
        
        public HearthbeatService(
            ILogger<HearthbeatService> logger,
            DefaultApplicationConfiguration configuration,
            IHearthbeatMessageProcessor processor,
            IChatnikPublisherSocket publisherSocket,
            TimeSpan? delay = null)
        {
            _logger = logger;
            _configuration = configuration;
            _publisher = publisherSocket;
            _processor = processor;
            SetEndlessRunner(delay.HasValue ? delay.Value : TimeSpan.FromSeconds(4));
        }
        
        protected override void Process()
        {
            _logger.LogInformation("Sending HB");
            _publisher.Send(new TransferMessage(_configuration.CurrentId.ToString()));
            
            ChangeStatus(!_respondedInCurrentRequest 
                ? ServerHearthbeatStatus.NotResponding
                : ServerHearthbeatStatus.Responding);
                
            _respondedInCurrentRequest = false;
        }

        public void StartListening(IMessageListener listener)
        {
            _logger.LogInformation("Starting to listen for hearthbeats");
            listener.SubscriberToTopic(_configuration.CurrentId.ToString(), _processor);
            _processor.MessageReceived += ProcessorOnMessageReceived;
            Run();
        }

        public event IHearthbeatService.OnServerNotResponding? ServerNotResponding;

        private void ChangeStatus(ServerHearthbeatStatus status)
        {
            if (status == Status)
                return;

            Status = status;
            ServerNotResponding?.Invoke(Status);
        }

        private void ProcessorOnMessageReceived()
        {
            _respondedInCurrentRequest = true;
            ChangeStatus(ServerHearthbeatStatus.Responding);
        }

        protected override void OnStopping()
        {
            Status = ServerHearthbeatStatus.Waiting;
            _respondedInCurrentRequest = true;
            
            _processor.MessageReceived -= ProcessorOnMessageReceived;
        }
    }
}