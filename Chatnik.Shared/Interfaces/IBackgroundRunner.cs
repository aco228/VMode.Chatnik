namespace Chatnik.Shared.Interfaces
{
    public interface IBackgroundRunner
    {
        bool IsRunning { get; }
        void Run();
        void Stop();
    }
}