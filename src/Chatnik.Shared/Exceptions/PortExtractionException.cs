using System;

namespace Chatnik.Shared.Exceptions
{
    public class PortExtractionException : Exception
    {
        public int SubscriptionPort { get; set; }
        public int PublisherPort { get; set; }
        public string Details { get; set; }
        
        public PortExtractionException(int subPort, int pubPort, string exceptionDetails)
        {
            SubscriptionPort = subPort;
            PublisherPort = pubPort;
            Details = exceptionDetails;
        }
    }
}