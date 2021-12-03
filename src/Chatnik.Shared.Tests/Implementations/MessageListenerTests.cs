using System;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;
using Chatnik.Shared.Tests.TestObjects.MessageProcessorsTestObjects;
using Chatnik.Tests.Shared;
using Chatnik.Tests.Shared.TestObjects.Models.Messages;
using Moq;
using Xunit;

namespace Chatnik.Shared.Tests.Implementations
{
    public class MessageListenerTests
    {
        [Fact]
        public void Should_Subscribe_And_Unsubscriber_To_Topic()
        {
            var (subscriber, messageListener) = TestHelper.GetMessageListener();
            var testMessageProcessor = new TestMessageProcessor();
            
            messageListener.SubscriberToTopic("test_topic", testMessageProcessor);
            Assert.Equal(1, messageListener.ProcessorsCount);
            
            messageListener.UnsubscribeFromTopic("test_topic");
            Assert.Equal(0, messageListener.ProcessorsCount);
        }

        [Fact]
        public void Should_Pass_Message_To_MessageProcessor()
        {
            var topicName = "topic";
            var (subscriber, messageListener) = TestHelper.GetMessageListener();
            var testMessageProcessor = new Mock<ITestMessageProcessor>();
            messageListener.SubscriberToTopic(topicName, testMessageProcessor.Object);
            
            var netMqMessage = UnitTestMessage.GetNetMqMessage(topic: topicName);
            subscriber.Setup(x => x.TryReceiveMessage()).Returns(new ReceiveMessage(netMqMessage));
            
            messageListener.Run();
            System.Threading.Thread.Sleep(2); // some delay just in case
            messageListener.Stop();

            testMessageProcessor.Verify(x => x.ProcessMessage(It.IsAny<IReceiveMessage>()), Times.AtLeastOnce);
        }

        [Fact]
        public void MessageProcessor_Should_Subscribe_To_All_Topics()
        {
            var (subscriber, messageListener) = TestHelper.GetMessageListener();
            var testMessageProcessor = new Mock<ITestMessageProcessor>();
            messageListener.SubscriberToTopic(string.Empty, testMessageProcessor.Object);

            subscriber.Setup(x => x.TryReceiveMessage())
                .Returns(new ReceiveMessage(UnitTestMessage.GetNetMqMessage(topic: Guid.NewGuid().ToString())));
            
            messageListener.Run();
            System.Threading.Thread.Sleep(5); // some delay just in case
            messageListener.Stop();

            testMessageProcessor.Verify(x => x.ProcessMessage(It.IsAny<IReceiveMessage>()), Times.AtLeastOnce);
        }

        [Fact]
        public void MessageProcessor_Should_Not_Receive_Messages_From_Topics_That_are_Not_Subscriber()
        {
            var topicName = "topic";
            var (subscriber, messageListener) = TestHelper.GetMessageListener();
            var testMessageProcessor = new Mock<ITestMessageProcessor>();
            messageListener.SubscriberToTopic(topicName, testMessageProcessor.Object);

            subscriber.Setup(x => x.TryReceiveMessage())
                .Returns(new ReceiveMessage(UnitTestMessage.GetNetMqMessage(topic: Guid.NewGuid().ToString())));
            
            messageListener.Run();
            System.Threading.Thread.Sleep(5); // some delay just in case
            messageListener.Stop();

            testMessageProcessor.Verify(x => x.ProcessMessage(It.IsAny<IReceiveMessage>()), Times.Never);
        }
    }
}