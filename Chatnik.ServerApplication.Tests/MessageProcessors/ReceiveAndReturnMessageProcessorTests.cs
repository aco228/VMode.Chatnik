using System;
using Chatnik.ServerApplication.MessageProcessors;
using Chatnik.Shared.Interfaces;
using Chatnik.Tests.Shared;
using Microsoft.Extensions.Logging;
using Moq;
using NetMQ;
using Xunit;

namespace Chatnik.ServerApplication.Tests.MessageProcessors
{
    public class ReceiveAndReturnMessageProcessorTests
    {

        [Fact]
        public void Should_Receive_And_Transfer_Message()
        {
            var (publisher, processor) = SetupMockObject();
            var (subscriber, messageListener) = TestHelper.GetMessageListener();

            subscriber.Setup(x => x.TryReceiveMessage()).Returns(TestHelper.GetReceiveMessage());
            
            // subscribe to all topics
            messageListener.SubscriberToTopic(string.Empty, processor);
            messageListener.Run();
            
            TestHelper.ContinueAfterDelay(25, () => messageListener.Stop());
            
            publisher.Verify(x => x.Send(It.IsAny<NetMQMessage>()), Times.AtLeastOnce);
        }

        private static Tuple<Mock<IChatnikPublisherSocket>, IReceiveAndReturnMessageProcessor> SetupMockObject()
        {
            var iloger = new Mock<ILogger<ReceiveAndReturnMessageProcessor>>();
            var subscriber = new Mock<IChatnikPublisherSocket>();
            var messageListener = new ReceiveAndReturnMessageProcessor(iloger.Object, subscriber.Object);
            return new Tuple<Mock<IChatnikPublisherSocket>, IReceiveAndReturnMessageProcessor>(subscriber, messageListener);
        }
    }
}