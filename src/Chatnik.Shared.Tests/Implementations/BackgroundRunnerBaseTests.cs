using System;
using System.Threading;
using Chatnik.Tests.Shared;
using Chatnik.Tests.Shared.TestObjects;
using Xunit;

namespace Chatnik.Shared.Tests.Implementations
{
    public class BackgroundRunnerBaseTests
    {
        [Fact]
        public void Should_Run_Background_Runner_When_Invoked()
        {
            var runner = new TestRunner();
            
            runner.Run();
            Thread.Sleep(5); // small delay
            
            Assert.Equal(1, runner.NumberOfInvocations);
        }
        
        [Fact]
        public void Should_Endless_Run_And_Stop()
        {
            var runner = new TestRunner(TimeSpan.FromMilliseconds(5));
            runner.Run();
            
            TestHelper.ContinueAfterDelay(10, () => Assert.True(runner.IsRunning));
            TestHelper.ContinueAfterDelay(6, () => runner.Stop());
            
            Thread.Sleep(1); // small delay
            
            Assert.True(runner.NumberOfInvocations > 1, "NumberOfInvocations should be more than 1");
            Assert.False(runner.IsRunning);
        }
    }
}