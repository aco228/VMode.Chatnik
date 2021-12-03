using System;
using Chatnik.ClientApplication.Core.MessageProcessors;
using Chatnik.Shared.Interfaces;
using Chatnik.Tests.Shared;
using Xunit;

namespace Chatnik.ClientApplication.Core.Tests.MessageProcessors
{
    public class HearthbeatMessageProcessorTests
    {
        [Fact]
        public void Should_Raise_Event_When_Hearthbeat_Response_is_Received()
        {
            var userId = Guid.NewGuid().ToString();
            var (subscriber, messageListener) = TestHelper.GetMessageListener();
            var processor = new HearthbeatMessageProcessor(TestHelper.GetLogger<HearthbeatMessageProcessor>().Object);
            
            var raisedMessageReceived = 0;
            processor.MessageReceived += () => ++raisedMessageReceived;
            
            subscriber.Setup(x => x.TryReceiveMessage())
                .Returns(GetHearthbeatMessage(topic: userId));
            
            messageListener.SubscriberToTopic(userId, processor);
            messageListener.Run();
            
            TestHelper.ContinueAfterDelay(15, () => messageListener.Stop());
            
            Assert.True(raisedMessageReceived > 0, "MessageReceived should be raised more than once");
        }
        
        private static IReceiveMessage GetHearthbeatMessage(
            string topic = "",
            string user = "aco")
            => TestHelper.GetReceiveMessage(
                topic,
                user);
    }
}