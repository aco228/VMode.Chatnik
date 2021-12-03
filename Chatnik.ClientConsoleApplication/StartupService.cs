using System.Threading;
using System.Threading.Tasks;
using Chatnik.ClientApplication.Core.Services;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;
using Microsoft.Extensions.Hosting;

namespace Chatnik.ClientConsoleApplication
{
    public class StartupService : IHostedService
    {
        private readonly DefaultApplicationConfiguration _configuration;
        private readonly IMessageListener _messageListener;
        private readonly IChatnikSubscriberSocket _subscriber;
        private readonly IChatnikPublisherSocket _publisher;
        private readonly IHearthbeatService _hearthbeatService;

        public StartupService(
            DefaultApplicationConfiguration configuration,
            IMessageListener messageListener,
            IChatnikPublisherSocket publisherSocket,
            IChatnikSubscriberSocket subscriberSocket,
            IHearthbeatService hearthbeatService)
        {
            _configuration = configuration;
            _messageListener = messageListener;
            _subscriber = subscriberSocket;
            _publisher = publisherSocket;
            _hearthbeatService = hearthbeatService;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriber.Configure(new() { Username = "aco", Address = _configuration.GetAddress(_configuration.SubscriberPort) });
            _publisher.Configure(new () { Username = "aco", Address = _configuration.GetAddress(_configuration.PublisherPort) });
            
            _messageListener.Run();
            _hearthbeatService.StartListening(_messageListener);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}