using System;
using System.Collections.Generic;
using Chatnik.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace Chatnik.Shared.Implementations
{
    public sealed class MessageListener : BackgroundRunnerBase, IMessageListener 
    {
        private readonly ILogger<MessageListener> _logger;
        private IChatnikSubscriberSocket _socket;
        private Dictionary<string, IMessageProcessor> _processors = new();

        public MessageListener(
            ILogger<MessageListener> logger,
            IChatnikSubscriberSocket socket)
        {
            _logger = logger;
            _socket = socket;
            
            SetEndlessRunner(TimeSpan.FromMilliseconds(5));
        }
        
        protected override void Process()
        {
            var message = _socket.TryReceiveMessage();
            if (message == null)
            {
                return;
            }
            
            if (string.IsNullOrEmpty(message.Topic))
            {
                _logger.LogCritical("Received message without topic!");
                return;
            }

            if (_processors.ContainsKey(string.Empty))
            {
                _processors[string.Empty].ProcessMessage(message);
            }
            
            if (_processors.ContainsKey(message.Topic))
            {
                _processors[message.Topic].ProcessMessage(message);
            }
        }

        public int ProcessorsCount { get => _processors.Count; } 

        public void SubscriberToTopic(string topicName, IMessageProcessor processor)
        {
            if(_processors.ContainsKey(topicName))
                return;

            _socket.SubscribeToTopic(topicName);
            _processors.Add(topicName, processor);
        }

        public void UnsubscribeFromTopic(string topicName)
        {
            if (!_processors.ContainsKey(topicName))
                return;

            _processors.Remove(topicName);
        }

        protected override void OnStopping()
        {
            _processors.Clear();
            _socket.Close();
        }
    }
}