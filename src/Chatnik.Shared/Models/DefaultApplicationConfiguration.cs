using System;
using Microsoft.Extensions.Configuration;

namespace Chatnik.Shared.Models
{
    public class DefaultApplicationConfiguration
    {
        public Guid CurrentId { get; set; } = Guid.NewGuid();
        public string[]? Args { get; set; }
        public string? BaseAddress { get; protected set; }
        public int SubscriberPort { get; protected set; }
        public int PublisherPort { get; protected set; }

        public DefaultApplicationConfiguration() { }
        
        public DefaultApplicationConfiguration(IConfiguration configuration)
        {
            SubscriberPort = configuration.GetValue<int>("DefaultSubscriberPort");
            PublisherPort = configuration.GetValue<int>("DefaultPublishedPort");
            BaseAddress = configuration.GetValue<string>("BaseAddress");
        }
        
        public string GetAddress(int port) => $"tcp://{BaseAddress}:{port}";
        public string GetAddress(string baseAddress, int port) => $"tcp://{baseAddress}:{port}";
    }
}