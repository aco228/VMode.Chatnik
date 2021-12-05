using System;
using System.Threading;
using Chatnik.Shared.Interfaces;

namespace Chatnik.Shared.Implementations
{
    public abstract class BackgroundRunnerBase : IBackgroundRunner
    {
        private bool _isRunning = false;
        private Thread? _thread = null;
        private TimeSpan? _delay = null;
        private CancellationTokenSource _cancellationTokenSource = new ();
        
        public bool IsRunning => _isRunning;

        /// <summary>
        /// 
        /// </summary>
        protected abstract void Process();
        protected virtual void OnStopping() { }

        protected void SetEndlessRunner(TimeSpan withDelay)
        {
            _delay = withDelay;
        }

        private void DoWork(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;
                
                Process();
                
                if (!_delay.HasValue)
                    return;

                cancellationToken.WaitHandle.WaitOne(_delay.Value);
                //Thread.Sleep(_delay.Value);
            }
        }

        public void Run()
        {
            if (_isRunning)
                return;
            
            _cancellationTokenSource = new CancellationTokenSource();
            _thread = new Thread(() =>
            {
                try
                {
                    _isRunning = true;
                    DoWork(_cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Cancellation requested");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception on worker {ex}");
                    throw ex;
                }
                finally
                {
                    _isRunning = false;
                    OnStopping();
                }
            });
            _thread.IsBackground = true;
            _thread.Start();
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}