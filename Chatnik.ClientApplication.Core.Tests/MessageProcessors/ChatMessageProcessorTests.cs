using System;
using Chatnik.ClientApplication.Core.MessageProcessors;
using Chatnik.Shared;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;
using Chatnik.Tests.Shared;
using Xunit;

namespace Chatnik.ClientApplication.Core.Tests.MessageProcessors
{
    public class ChatMessageProcessorTests
    {
        [Fact]
        public void Should_Raise_Event_When_ChatMessage_Is_Received()
        {
            var (subscriber, messageListener) = TestHelper.GetMessageListener();
            var processor = new ChatMessageProcessor();

            var raisedMessageReceived = 0;
            processor.MessageReceived += message => ++raisedMessageReceived;  

            subscriber.Setup(x => x.TryReceiveMessage())
                .Returns(GetChatReceiveMessage(text: Guid.NewGuid().ToString()));
            
            messageListener.SubscriberToTopic(GlobalConstants.MainChannel, processor);
            messageListener.Run();
            
            TestHelper.ContinueAfterDelay(15, () => messageListener.Stop());
            
            Assert.True(raisedMessageReceived > 0, "MessageReceived should be raised more than once");
        }

        private static IReceiveMessage GetChatReceiveMessage(
            string text = "random text",
            DateTime? dateTime = null,
            ChatMessageType type = ChatMessageType.Message,
            string user = "aco")
            => TestHelper.GetReceiveMessage(
                topic: GlobalConstants.MainChannel,
                user: user,
                text,
                (dateTime.HasValue ? dateTime.ToString() : DateTime.Now.ToString()),
                type.ToString());
    }
}