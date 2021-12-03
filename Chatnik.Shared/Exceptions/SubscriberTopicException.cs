using System;

namespace Chatnik.Shared.Exceptions
{
    public class SubscriberTopicException : Exception
    {
        public string TopicName { get; set; }
        public string Reason { get; set; }
        
        public SubscriberTopicException(string topicName, string reason)
        {
            TopicName = topicName;
            Reason = reason;
        }
    }
}