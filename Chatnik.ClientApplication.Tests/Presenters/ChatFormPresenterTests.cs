using System;
using Chatnik.ClientApplication.Core.MessageProcessors;
using Chatnik.ClientApplication.Core.Services;
using Chatnik.ClientApplication.Presenters;
using Chatnik.ClientApplication.Views;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;
using Moq;
using Xunit;

namespace Chatnik.ClientApplication.Tests.Presenters
{
    public class ChatFormPresenterTests
    {

        [Fact]
        public void OnLoad_Should_Send_UserJoined_Event()
        {
            var (view, publisher, presenter) = GetPresenter();
            
            presenter.OnFormLoaded();
            publisher.Verify(x => x.Send(It.Is<ChatMessage>(x => x.Type == ChatMessageType.UserJoined)), Times.Once);
        }
        
        [Fact]
        public void OnClosing_Should_Send_UserLeft_Event()
        {
            var (view, publisher, presenter) = GetPresenter();
            
            presenter.OnClosing();
            
            publisher.Verify(x => x.Send(It.Is<ChatMessage>(x => x.Type == ChatMessageType.UserLeft)), Times.Once);
        }
        
        private static Tuple<Mock<IChatView>, Mock<IChatnikPublisherSocket>, ChatFormPresenter> GetPresenter()
        {
            var publisher = new Mock<IChatnikPublisherSocket>();
            var messageListener = new Mock<IMessageListener>();
            var chatMessageProcessor = new Mock<IChatMessageProcessor>();
            var hearthbeatService = new Mock<IHearthbeatService>();
            var view = new Mock<IChatView>();
            var presenter = new ChatFormPresenter(publisher.Object, messageListener.Object, chatMessageProcessor.Object, hearthbeatService.Object, view.Object);
            return new (view, publisher, presenter);
        }
    }
}