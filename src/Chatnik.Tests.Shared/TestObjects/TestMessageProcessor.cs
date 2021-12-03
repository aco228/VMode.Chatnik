using Chatnik.Shared.Implementations;
using Chatnik.Shared.Interfaces;
using Chatnik.Tests.Shared.TestObjects.Models.Messages;

namespace Chatnik.Shared.Tests.TestObjects.MessageProcessorsTestObjects
{
    public interface ITestMessageProcessor : IMessageProcessor
    {
        
    }
    
    public class TestMessageProcessor : MessageProcessorBase<UnitTestMessage>, ITestMessageProcessor
    {
        protected override void Process(UnitTestMessage message)
        {
            // No need to implement anything
        }
    }
}