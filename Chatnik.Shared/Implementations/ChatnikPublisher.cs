using Chatnik.Shared.Helpers;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;
using NetMQ;
using NetMQ.Sockets;

namespace Chatnik.Shared.Implementations
{
    public class ChatnikPublisher : IChatnikPublisherSocket
    {
        private PublisherSocket? _socket = null;
        private string _address = string.Empty;
        private string _username = string.Empty;

        public INetMQSocket? Socket { get => _socket; }
        public bool IsConfigured { get => _socket != null && !_socket.IsDisposed; }
        public string CurrentUser { get => _username; }
        
        public void Dispose()
        {
            if (_socket == null)
                return;
            
            if (!string.IsNullOrEmpty(_address))
                _socket.Disconnect(_address);
            
            _socket.Dispose();
        }

        public void Configure(SocketConfigureModel configureModel)
        {
            _username = configureModel.Username;
            _address = configureModel.Address;
            _socket = new PublisherSocket();
            _socket.Bind(_address);
            
            // Delay in order to establish connection (TODO: check fi this is correct) 
            System.Threading.Thread.Sleep(500);
        }

        public void Close()
        {
            Dispose();
        }
        
        public void Send(ITransferMessage message)
        {
            // TODO: Consider throwing exception
            if (!IsConfigured)
                return;
            
            if (string.IsNullOrEmpty(message.User))
                message.User = _username;
            
            Send(message.Convert());   
        }

        public void Send(NetMQMessage message)
        {
            // TODO: Consider throwing exception
            if (!IsConfigured)
                return;
            
            _socket.SendMultipartMessage(message);
        }
    }
}