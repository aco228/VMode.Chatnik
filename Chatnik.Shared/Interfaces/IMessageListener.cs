namespace Chatnik.Shared.Interfaces
{
    public interface IMessageListener : IBackgroundRunner
    {
        public int ProcessorsCount { get; }
        public void SubscriberToTopic(string topicName, IMessageProcessor processor);
        public void UnsubscribeFromTopic(string topicName);
    }
}