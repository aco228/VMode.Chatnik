using System;

namespace Chatnik.Shared.Interfaces
{
    public interface IChatnikSubscriberSocket : IChatnikSocket
    {
        public void SubscribeToTopic(string topicName);
        public void UnsubscribeToTopic(string topicName);
        public IReceiveMessage? TryReceiveMessage();
        public IReceiveMessage? TryReceiveMessage(TimeSpan maximumDelay);
    }
}