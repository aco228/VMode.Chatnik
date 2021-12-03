using System;
using Chatnik.ClientApplication.Core.Implementations;
using Chatnik.ClientApplication.Core.Interfaces;
using Chatnik.ClientApplication.Core.MessageProcessors;
using Chatnik.ClientApplication.Core.Models;
using Chatnik.ClientApplication.Core.Services;
using Chatnik.ClientApplication.Views;
using Chatnik.Shared;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;

namespace Chatnik.ClientApplication.Presenters
{
    public interface IChatFormPresenter : IPresenter
    {
        void OnSubmitMessageClick();
        void OnClosing();
    }
    
    public class ChatFormPresenter : PresenterBase<IChatView>, IChatFormPresenter
    {
        private readonly IChatnikPublisherSocket _publisher;
        private readonly IMessageListener _messageListener;
        private readonly IChatMessageProcessor _chatMessageProcessor;
        private readonly IHearthbeatService _hearthbeatService;

        private ServerHearthbeatStatus _currentServerStatus = ServerHearthbeatStatus.Responding;
        
        public ChatFormPresenter(
            IChatnikPublisherSocket publisherSocket,
            IMessageListener messageListener,
            IChatMessageProcessor chatMessageProcessor,
            IHearthbeatService hearthbeatService,
            IChatView view) 
            : base(view)
        {
            _publisher = publisherSocket;
            _chatMessageProcessor = chatMessageProcessor;
            _hearthbeatService = hearthbeatService;
            _messageListener = messageListener;
        }

        public override void OnFormLoaded()
        {
            _messageListener.Run();
            _messageListener.SubscriberToTopic(GlobalConstants.MainChannel, _chatMessageProcessor);
            _hearthbeatService.StartListening(_messageListener);
            
            _publisher.Send(new ChatMessage{ Type = ChatMessageType.UserJoined });
            
            _chatMessageProcessor.MessageReceived += ChatMessageProcessorOnMessageReceived;
            _hearthbeatService.ServerNotResponding += HearthbeatServiceOnServerNotResponding; 
        }

        private void HearthbeatServiceOnServerNotResponding(ServerHearthbeatStatus status)
        {
            if (_currentServerStatus == status)
                return;

            if (_currentServerStatus == ServerHearthbeatStatus.NotResponding &&
                status == ServerHearthbeatStatus.Responding)
            {
                View.OnChatMessageReceived(new() { Type = ChatMessageType.ServerRegainedConnection });
                View.OnServerStartResponding();
            }

            if (_currentServerStatus == ServerHearthbeatStatus.Responding &&
                status == ServerHearthbeatStatus.NotResponding)
            {
                View.OnChatMessageReceived(new() { Type = ChatMessageType.ServerLostConnection });
                View.OnServerNotResponding();   
            }

            _currentServerStatus = status;
        }

        private void ChatMessageProcessorOnMessageReceived(ChatMessage message)
            => View.OnChatMessageReceived(message);

        public void OnSubmitMessageClick()
        {
            if (_currentServerStatus != ServerHearthbeatStatus.Responding)
                return;
            
            if (string.IsNullOrEmpty(View.ChatMessage))
                return;

            _publisher.Send(new ChatMessage
            {
                Text = View.ChatMessage,
                Type = ChatMessageType.Message,
                Date = DateTime.Now
            });
            View.ChatMessage = string.Empty;
        }

        public void OnClosing()
        {
            _publisher.Send(new ChatMessage{ Type = ChatMessageType.UserLeft });
            _messageListener.UnsubscribeFromTopic(GlobalConstants.MainChannel);
            _messageListener.Stop();
            _hearthbeatService.Stop();
            _publisher.Close();
            
            _chatMessageProcessor.MessageReceived -= ChatMessageProcessorOnMessageReceived;
            _hearthbeatService.ServerNotResponding -= HearthbeatServiceOnServerNotResponding;
        }
    }
}