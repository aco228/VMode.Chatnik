using System.Threading;
using System.Threading.Tasks;
using Chatnik.ServerApplication.Helpers;
using Chatnik.ServerApplication.MessageProcessors;
using Chatnik.Shared.Helpers;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Chatnik.ServerApplication
{
    public class StartupService : IHostedService
    {
        private readonly ILogger<StartupService> _logger;
        private readonly DefaultApplicationConfiguration _defaultApplicationConfiguration;
        private readonly IChatnikSubscriberSocket _subscriber;
        private readonly IChatnikPublisherSocket _publisher;
        private readonly IMessageListener _messageListener;
        private readonly IPortTester _portTester;
        private readonly IReceiveAndReturnMessageProcessor _receiveAndReturnMessageProcessor;

        public StartupService(
            ILogger<StartupService> logger,
            DefaultApplicationConfiguration defaultApplicationConfiguration,
            IChatnikPublisherSocket publisherSocket,
            IChatnikSubscriberSocket subscriberSocket,
            IMessageListener messageListener,
            IPortTester portTester,
            IReceiveAndReturnMessageProcessor receiveAndReturnMessageProcessor)
        {
            _logger = logger;
            _defaultApplicationConfiguration = defaultApplicationConfiguration;
            _publisher = publisherSocket;
            _subscriber = subscriberSocket;
            _messageListener = messageListener;
            _receiveAndReturnMessageProcessor = receiveAndReturnMessageProcessor;
            _portTester = portTester;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
           if (!TryToConfigureSockets())
               return;
            
           _messageListener.Run();
           _messageListener.SubscriberToTopic(string.Empty, _receiveAndReturnMessageProcessor);
           _logger.LogInformation("Starting to listen");
        }

        private bool TryToConfigureSockets()
        {
            var (publisherPort, subscriberPort) = PortsExtractionsHelper.ReadPorts(_defaultApplicationConfiguration);
            
            if (!_portTester.TestPort(_defaultApplicationConfiguration.BaseAddress, publisherPort))
            {
                _logger.LogCritical($"ERR: PUBLIHED port {publisherPort} on address {_defaultApplicationConfiguration.BaseAddress} have problem");
                return false;
            }
            
            _subscriber.Configure(new()
            {
                Address = _defaultApplicationConfiguration.GetAddress(subscriberPort)
            });
            
            _publisher.Configure(new ()
            {
                Address = _defaultApplicationConfiguration.GetAddress(publisherPort)
            });

            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}