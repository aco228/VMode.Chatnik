using Chatnik.ClientApplication.Core.Interfaces;
using Chatnik.Shared.Models;

namespace Chatnik.ClientApplication.Views
{
    public interface IChatView : IView
    {
        public string ChatMessage { get; set; }

        void OnChatMessageReceived(ChatMessage message);
        void OnServerNotResponding();
        void OnServerStartResponding();
    }
}