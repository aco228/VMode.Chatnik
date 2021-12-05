using System;
using Chatnik.Shared.Models;
using NetMQ;

namespace Chatnik.Shared.Interfaces
{
    public interface IChatnikSocket : IDisposable
    {
        public int Port { get; }
        public INetMQSocket? Socket { get; }
        public bool IsConfigured { get; }
        void Configure(SocketConfigureModel configureModel);
        void Close();
    }
}