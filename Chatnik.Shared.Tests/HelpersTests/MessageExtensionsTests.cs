using System.Text.Json;
using Chatnik.Shared.Exceptions;
using Chatnik.Shared.Helpers;
using Chatnik.Shared.Models;
using Chatnik.Tests.Shared.TestObjects.Models.Messages;
using NetMQ;
using Xunit;

namespace Chatnik.Shared.Tests.Helpers
{
    public class MessageExtensionsTests
    {
        [Fact]
        public void Should_Serialize_Message()
        {
            var netMqMessage = new NetMQMessage();

            var property_test1 = "test1";
            var property_test2 = "test2";
            
            netMqMessage.Append("topic");
            netMqMessage.Append("user");
            netMqMessage.Append(property_test1);
            netMqMessage.Append(property_test2);

            var messageResponse = new ReceiveMessage(netMqMessage);
            var convertedObject = messageResponse.Convert<UnitTestMessage>();
            
            Assert.Equal(convertedObject.Test1, property_test1);
            Assert.Equal(convertedObject.Test2, property_test2);
        }

        [Fact]
        public void Should_Seriliaze_Json_Data_Object_Property()
        {
            var netMqMessage = new NetMQMessage();
            var name = "Some name";
            var data = new UnitTestWithObjectSerializationObj { Name = "Name Inside of object" };
            
            netMqMessage.Append("topic");
            netMqMessage.Append("user");
            netMqMessage.Append(name);
            netMqMessage.Append(JsonSerializer.Serialize(data));
            
            var messageResponse = new ReceiveMessage(netMqMessage);
            var convertedObject = messageResponse.Convert<UnitTestWithObjectSerializationMessage>();
            
            Assert.Equal(convertedObject.Name, name);
            Assert.Equal(convertedObject.Data.Name, data.Name);
        }

        [Fact]
        public void Should_Convert_MessageBase_ToNetMqMessage()
        {
            var message = new UnitTestTransferMessage
            {
                Topic = "topic",
                User = "user",
                Test1 = "prvi test",
                Test2 = "drugi test"
            };

            var messageMq = message.Convert();
            
            Assert.Equal(4, messageMq.FrameCount);
            Assert.Equal(message.Topic, messageMq.Pop().ConvertToString());
            Assert.Equal(message.User, messageMq.Pop().ConvertToString());
            Assert.Equal(message.Test1, messageMq.Pop().ConvertToString());
            Assert.Equal(message.Test2, messageMq.Pop().ConvertToString());
        }

        [Fact]
        public void Should_Throw_Exception_If_Positions_Are_Wrong()
        {
            var netMqMessage = new NetMQMessage();
            netMqMessage.Append("topic");
            netMqMessage.Append("user");
            netMqMessage.Append("property_test1");

            var messageResponse = new ReceiveMessage(netMqMessage);

            Assert.Throws<MessageBaseSerializationException>(() => messageResponse.Convert<UnitTestMessage>());
        }
        
    }
}