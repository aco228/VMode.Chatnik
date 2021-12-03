using System;
using Chatnik.ClientApplication.Core.MessageProcessors;
using Chatnik.ClientApplication.Core.Models;
using Chatnik.ClientApplication.Core.Services;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;
using Chatnik.Tests.Shared;
using Moq;
using Xunit;

namespace Chatnik.ClientApplication.Core.Tests.Services
{
    public class HearthbeatServiceTests
    {
        public Guid CurrentUserId = Guid.NewGuid();

        [Fact]
        public void Should_Send_Hearthbeat_Messages()
        {
            var (processor, publisher, service) = GetService();
            service.Run();
            
            TestHelper.ContinueAfterDelay(3, () => service.Stop());
            
            publisher.Verify(x => x.Send(It.IsAny<ITransferMessage>()), Times.AtLeastOnce);
        }

        [Fact]
        public void Should_Change_Status_Based_On_Hearthbeat_Response()
        {
            var userId = CurrentUserId.ToString();
            var (processor, publisher, service) = GetService();
            var (subsciber, messageListener) = TestHelper.GetMessageListener();
            service.StartListening(messageListener);

            var timesCalledServerNotResponding = 0;
            service.ServerNotResponding += status => ++timesCalledServerNotResponding; 

            TestHelper.ContinueAfterDelay(6, () => Assert.Equal(ServerHearthbeatStatus.NotResponding, service.Status));
            processor.ProcessMessage(TestHelper.GetReceiveMessage(userId, userId));
            TestHelper.ContinueAfterDelay(2, () => service.Stop());
            
            Assert.Equal(ServerHearthbeatStatus.Responding, service.Status);
            Assert.True(timesCalledServerNotResponding > 2, "ServerNotResponding should be called more than 2 times");
        }

        private Tuple<IHearthbeatMessageProcessor, Mock<IChatnikPublisherSocket>, HearthbeatService> GetService()
        {
            var processor = new HearthbeatMessageProcessor(TestHelper.GetLogger<HearthbeatMessageProcessor>().Object);
            var publisher = new Mock<IChatnikPublisherSocket>();
            var logger = TestHelper.GetLogger<HearthbeatService>();
            var configuration = new DefaultApplicationConfiguration();
            configuration.CurrentId = CurrentUserId; 
            var service = new HearthbeatService(logger.Object, configuration, processor, publisher.Object, TimeSpan.FromMilliseconds(1));
            
            return new Tuple<IHearthbeatMessageProcessor, Mock<IChatnikPublisherSocket>, HearthbeatService>(
                processor,
                publisher, 
                service);
        }
    }
}