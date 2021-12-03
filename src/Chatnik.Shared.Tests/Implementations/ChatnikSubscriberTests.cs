using Chatnik.Shared.Exceptions;
using Chatnik.Shared.Implementations;
using Xunit;

namespace Chatnik.Shared.Tests.Implementations
{
    public class ChatnikSubscriberTests
    {
        [Fact]
        public void Should_Trow_Exception_When_Not_Configured_And_Try_To_Subscibe_To_Socket()
        {
            var socket = new ChatnikSubscriber();
            Assert.Throws<SubscriberTopicException>(() => socket.SubscribeToTopic("a"));
        }
        
        [Fact]
        public void Should_Trow_Exception_When_Not_Configured_And_Try_To_UnSubscibe_To_Socket()
        {
            var socket = new ChatnikSubscriber();
            Assert.Throws<SubscriberTopicException>(() => socket.UnsubscribeToTopic("a"));
        }
    }
}