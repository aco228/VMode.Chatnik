using System;
using Chatnik.ClientApplication.Presenters;
using Chatnik.ClientApplication.Views;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;
using Moq;
using NetMQ;
using Xunit;

namespace Chatnik.ClientApplication.Tests.Presenters
{
    public class LoginFormPresenterTests
    {
        public static Guid CurrentId = Guid.NewGuid();
        
        [Theory]
        [InlineData("", "", "", "")]
        [InlineData("aco", "", "", "")]
        [InlineData("aco", "127.0.0.1", "", "")]
        [InlineData("aco", "127.0.0.1", "wrongPublisher", "")]
        [InlineData("aco", "127.0.0.1", "15", "")]
        [InlineData("aco", "127.0.0.1", "15", "wrongSubscriber")]
        public void OnLoginButtonClick_Should_Raise_Error_For_Wrong_Inputs(
            string username, 
            string remoteAddress, 
            string publiherPort, 
            string subscriberPort)
        {
            var (view, presenter) = GetLoginPresenter();
            SetValuesInView(view, username, remoteAddress, publiherPort, subscriberPort);
            presenter.OnLoginButtonClick();

            view.Verify(x => x.OnLoginError(It.IsAny<string>()), Times.Once);
            view.Verify(x => x.OnLoginSuccess(), Times.Never);
        }

        [Fact]
        public void OnLoginButton_Should_Return_Error_If_Port_Is_Unreachable()
        {
            var (view, publisher, subscriber, portTester, presenter) = GetLoginPresenterWithSockets();
            SetValuesInView(view, "username", "remoteAddress", "11", "25");

            portTester.Setup(x => x.TestPort(It.IsAny<string>(), It.IsAny<int>())).Returns(false);
            subscriber.Setup(x => x.TryReceiveMessage(It.IsAny<TimeSpan>())).Returns((IReceiveMessage)null);
            
            presenter.OnLoginButtonClick();
            
            subscriber.Verify(x => x.SubscribeToTopic(It.IsAny<string>()), Times.Never);
            publisher.Verify(x => x.Send(It.IsAny<ITransferMessage>()), Times.Never);
            subscriber.Verify(x => x.UnsubscribeToTopic(It.IsAny<string>()), Times.Never);
            
            view.Verify(x => x.OnLoginError(It.IsAny<string>()), Times.Once);
            view.Verify(x => x.OnLoginSuccess(), Times.Never);
        }

        [Fact]
        public void OnLoginButton_Should_Return_Error_If_Server_Is_Unreachable()
        {
            var (view, publisher, subscriber, portTester, presenter) = GetLoginPresenterWithSockets();
            SetValuesInView(view, "username", "remoteAddress", "11", "25");

            portTester.Setup(x => x.TestPort(It.IsAny<string>(), It.IsAny<int>())).Returns(true);
            subscriber.Setup(x => x.TryReceiveMessage(It.IsAny<TimeSpan>())).Returns((IReceiveMessage)null);
            
            presenter.OnLoginButtonClick();
            
            subscriber.Verify(x => x.SubscribeToTopic(It.IsAny<string>()), Times.Once);
            publisher.Verify(x => x.Send(It.IsAny<ITransferMessage>()), Times.Once);
            
            // no unsubscribe, as not server
            subscriber.Verify(x => x.UnsubscribeToTopic(It.IsAny<string>()), Times.Never);
            
            view.Verify(x => x.OnLoginError(It.IsAny<string>()), Times.Once);
            view.Verify(x => x.OnLoginSuccess(), Times.Never);
        }

        [Fact]
        public void OnLoginButton_Should_Go_To_Success_If_Everything_Is_Okay()
        {
            var (view, publisher, subscriber, portTester, presenter) = GetLoginPresenterWithSockets();
            SetValuesInView(view, "username", "remoteAddress", "11", "25");

            portTester.Setup(x => x.TestPort(It.IsAny<string>(), It.IsAny<int>())).Returns(true);
            subscriber.Setup(x => x.TryReceiveMessage(It.IsAny<TimeSpan>())).Returns(GetReceiveMessage);
            
            presenter.OnLoginButtonClick();
            
            subscriber.Verify(x => x.SubscribeToTopic(It.IsAny<string>()), Times.Once);
            publisher.Verify(x => x.Send(It.IsAny<ITransferMessage>()), Times.Once);
            subscriber.Verify(x => x.UnsubscribeToTopic(It.IsAny<string>()), Times.Once);
            
            view.Verify(x => x.OnLoginError(It.IsAny<string>()), Times.Never);
            view.Verify(x => x.OnLoginSuccess(), Times.Once);
        }

        private static ReceiveMessage GetReceiveMessage()
        {
            var mqMessage = new NetMQMessage();
            mqMessage.Append(CurrentId.ToString());
            mqMessage.Append(CurrentId.ToString());
            return new ReceiveMessage(mqMessage);
        }

        private static void SetValuesInView(Mock<ILoginView> view, string username, string remoteAddress, string publisherPort,
            string subscriberPort)
        {
            
            view.Setup(x => x.Username).Returns(username);
            view.Setup(x => x.RemoteAddress).Returns(remoteAddress);
            view.Setup(x => x.PublisherPort).Returns(publisherPort);
            view.Setup(x => x.SubscriberPort).Returns(subscriberPort);
        }

        private static Tuple<Mock<ILoginView>, LoginFormPresenter> GetLoginPresenter()
        {
            var configuration = new DefaultApplicationConfiguration();
            configuration.CurrentId = CurrentId;
            var publiher = new Mock<IChatnikPublisherSocket>();
            var subscriber = new Mock<IChatnikSubscriberSocket>();
            var portTester = new Mock<IPortTester>();
            var loginView = new Mock<ILoginView>();
            var presenter = new LoginFormPresenter(configuration, publiher.Object, subscriber.Object, portTester.Object, loginView.Object);
            return new (loginView, presenter);
        }
        
        private static Tuple<Mock<ILoginView>, Mock<IChatnikPublisherSocket>, Mock<IChatnikSubscriberSocket>, Mock<IPortTester>, LoginFormPresenter> 
            GetLoginPresenterWithSockets()
        {
            var configuration = new DefaultApplicationConfiguration();
            configuration.CurrentId = CurrentId;
            var publiher = new Mock<IChatnikPublisherSocket>();
            var subscriber = new Mock<IChatnikSubscriberSocket>();
            var portTester = new Mock<IPortTester>();
            var loginView = new Mock<ILoginView>();
            var presenter = new LoginFormPresenter(configuration, publiher.Object, subscriber.Object, portTester.Object, loginView.Object);
            return new (loginView, publiher, subscriber, portTester, presenter);
        }
    }
}