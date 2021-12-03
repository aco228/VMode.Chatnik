using Chatnik.Shared.Models;

namespace Chatnik.Tests.Shared.TestObjects.Models.Messages
{
    public class UnitTestWithObjectSerializationObj
    {
        public string Name { get; set; }

        public UnitTestWithObjectSerializationObj()
        {
            
        }
    }
    
    public class UnitTestWithObjectSerializationMessage : TransferMessage
    {
        public string Name { get; set; }
        public UnitTestWithObjectSerializationObj Data { get; set; }

        public UnitTestWithObjectSerializationMessage(string topic) : base(topic)
        {
        }
    }
}