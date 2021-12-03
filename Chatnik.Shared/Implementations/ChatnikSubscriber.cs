using System;
using Chatnik.Shared.Exceptions;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;
using NetMQ;
using NetMQ.Sockets;

namespace Chatnik.Shared.Implementations
{
    public class ChatnikSubscriber : IChatnikSubscriberSocket
    {
        private SubscriberSocket? _socket = null;
        private string _address = string.Empty;

        public INetMQSocket? Socket { get => _socket; }
        public bool IsConfigured { get => _socket != null && !_socket.IsDisposed; }

        public void Dispose()
        {
            if(_socket == null)
                return;
            
            if (!string.IsNullOrEmpty(_address))
                _socket.Disconnect(_address);
            
            _socket.Dispose();
        }

        public void Configure(SocketConfigureModel configureModel)
        {
            _address = configureModel.Address;
            _socket = new SubscriberSocket();
            _socket.Connect(_address);
        }

        public void Close()
        {
            Dispose();
        }

        public void SubscribeToTopic(string topicName)
        {
            if (!IsConfigured)
                throw new SubscriberTopicException(topicName, "Socket is not configured");
            
            if (string.IsNullOrEmpty(topicName))
                _socket.SubscribeToAnyTopic();
            else
                _socket.Subscribe(topicName);
        }

        public void UnsubscribeToTopic(string topicName)
        {
            if (!IsConfigured)
                throw new SubscriberTopicException(topicName, "Socket is not configured");
            
            _socket.Unsubscribe(topicName);
        }

        public IReceiveMessage? TryReceiveMessage()
            => TryReceiveMessage(TimeSpan.Zero);
        
        public IReceiveMessage? TryReceiveMessage(TimeSpan maximumDelay)
        {
            if (!IsConfigured)
                return null;

            var message = new NetMQMessage();
            if (!_socket.TryReceiveMultipartMessage(maximumDelay, ref message))
            {
                return null;
            }

            return new ReceiveMessage(message);
        }
    }
}