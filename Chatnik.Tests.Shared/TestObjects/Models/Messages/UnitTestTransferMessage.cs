using Chatnik.Shared.Models;

namespace Chatnik.Tests.Shared.TestObjects.Models.Messages
{
    public class UnitTestTransferMessage : TransferMessage
    {
        public string Test1 { get; set; }
        public string Test2 { get; set; }

        public UnitTestTransferMessage() : base("topic") { }
    }
}