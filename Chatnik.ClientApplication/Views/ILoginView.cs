using Chatnik.ClientApplication.Core.Interfaces;

namespace Chatnik.ClientApplication.Views
{
    public interface ILoginView : IView
    {
        public string RemoteAddress { get; set; }
        public string SubscriberPort { get; set; }
        public string PublisherPort { get; set; }
        public string Username { get; set; }

        void SetLoading(bool isLoading);
        void OnLoginError(string errorMessage);
        void OnLoginSuccess();
    }
}