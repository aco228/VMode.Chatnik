using System;
using System.Net;
using System.Net.Sockets;
using Chatnik.Shared.Exceptions;
using Chatnik.Shared.Models;

namespace Chatnik.ServerApplication.Helpers
{
    public static class PortsExtractionsHelper
    {
        
        public static (int PublisherPort, int SubscriberPort) ReadPorts(DefaultApplicationConfiguration defaultConfiguraion)
        {
            if (defaultConfiguraion.Args == null || defaultConfiguraion.Args.Length == 0)
                return (defaultConfiguraion.PublisherPort, defaultConfiguraion.SubscriberPort);

            var arg1 = (defaultConfiguraion.Args != null && defaultConfiguraion.Args.Length > 0) ? defaultConfiguraion.Args[0] : null;
            var arg2 = (defaultConfiguraion.Args != null && defaultConfiguraion.Args.Length > 1) ? defaultConfiguraion.Args[1] : null;

            var publisherPort = ReadPortFromConsole("Provide port number for publisher socket: ", arg1);
            var subscriberPort = ReadPortFromConsole("Provide port number for subscriber socket: ", arg2);

            if (publisherPort == subscriberPort)
                throw new PortExtractionException(subscriberPort, publisherPort, "Port number cannot be the same");

            return (publisherPort, subscriberPort);
        }

        private static int ReadPortFromConsole(string message, string? arg)
        {
            int port;
            while (!int.TryParse(arg, out port))
            {
                Console.WriteLine(message);
                arg = Console.ReadLine();
            }
            return port;
        }
    }
}