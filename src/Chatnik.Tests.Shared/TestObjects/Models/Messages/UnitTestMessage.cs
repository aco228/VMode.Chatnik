using Chatnik.Shared.Models;
using NetMQ;

namespace Chatnik.Tests.Shared.TestObjects.Models.Messages
{
    public class UnitTestMessage : TransferMessage
    {
        public string Test1 { get; set; }
        public string Test2 { get; set; }

        public UnitTestMessage(string topic) : base(topic)
        {
        }

        public static NetMQMessage GetNetMqMessage(
            string topic = "topic",
            string user = "user",
            string test1 = "test1",
            string test2 = "test2")
        {
            var message = new NetMQMessage();
            message.Append(topic);
            message.Append(user);
            message.Append(test1);
            message.Append(test2);
            return message;
        }
    }
}