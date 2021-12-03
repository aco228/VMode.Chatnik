using System;
using System.Threading;
using Chatnik.Shared.Implementations;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NetMQ;

namespace Chatnik.Tests.Shared
{
    public static class TestHelper
    {
        public static Tuple<Mock<IChatnikSubscriberSocket>, IMessageListener> GetMessageListener()
        {
            var iloger = new Mock<ILogger<MessageListener>>();
            var subscriber = new Mock<IChatnikSubscriberSocket>();
            var messageListener = new MessageListener(iloger.Object, subscriber.Object);
            return new Tuple<Mock<IChatnikSubscriberSocket>, IMessageListener>(subscriber, messageListener);
        }

        public static Mock<ILogger<T>> GetLogger<T>()
            => new ();
        
        public static void ContinueAfterDelay(int miliseconds, Action action)
        {
            Thread.Sleep(miliseconds);
            action();
        }

        public static IReceiveMessage GetReceiveMessage(
            string? topic = null,
            string user = "user",
            params string[] frames)
            => new ReceiveMessage(GNetMqMessage(topic, user, frames));
        
        public static NetMQMessage GNetMqMessage(
            string? topic = null,
            string user = "user",
            params string[] frames)
        {
            topic ??= Guid.NewGuid().ToString();
            
            var message = new NetMQMessage();
            message.Append(topic);
            message.Append(user);
            
            foreach (var frame in frames)
                message.Append(frame);
            
            return message;
        }
    }
}