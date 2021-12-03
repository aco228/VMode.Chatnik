using System;
using Chatnik.Shared.Implementations;

namespace Chatnik.Tests.Shared.TestObjects
{
    public class TestRunner : BackgroundRunnerBase
    {
        public int NumberOfInvocations { get; private set; } = 0;

        public TestRunner(TimeSpan? endlessRunnerDelay = null)
        {
            if (endlessRunnerDelay.HasValue)
                SetEndlessRunner(endlessRunnerDelay.Value);
        }
        
        protected override void Process()
        { 
            // do nothing
            ++NumberOfInvocations;
        }
    }
}