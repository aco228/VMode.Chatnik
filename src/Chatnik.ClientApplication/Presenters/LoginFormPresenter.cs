using System;
using Chatnik.ClientApplication.Core.Implementations;
using Chatnik.ClientApplication.Core.Interfaces;
using Chatnik.ClientApplication.Views;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;

namespace Chatnik.ClientApplication.Presenters
{
    public interface ILoginFormPresenter : IPresenter
    {
        void OnLoginButtonClick();
    }
    
    public class LoginFormPresenter : PresenterBase<ILoginView>, ILoginFormPresenter
    {
        private readonly DefaultApplicationConfiguration _configuration;
        private readonly IChatnikPublisherSocket _publisher;
        private readonly IChatnikSubscriberSocket _subscriber;
        private readonly IPortTester _portTester;
        
        public LoginFormPresenter(
            DefaultApplicationConfiguration configuration,
            IChatnikPublisherSocket publisherSocket,
            IChatnikSubscriberSocket subscriberSocket,
            IPortTester portTester,
            ILoginView view) 
            : base(view)
        {
            _subscriber = subscriberSocket;
            _publisher = publisherSocket;
            _configuration = configuration;
            _portTester = portTester;
        }

        public override void OnFormLoaded()
        {
            View.RemoteAddress = _configuration.BaseAddress;
            View.PublisherPort = _configuration.PublisherPort.ToString();
            View.SubscriberPort = _configuration.SubscriberPort.ToString();
        }

        public void OnLoginButtonClick()
        {
            if (string.IsNullOrEmpty(View.Username))
            {
                View.OnLoginError("Username is emptry");
                return;
            }

            if (string.IsNullOrEmpty(View.RemoteAddress))
            {
                // TODO: Add more logic 
                View.OnLoginError("RemoteAddress is empty");
                return;
            }

            if (!PortIsValid(View.PublisherPort, true, out var publisherErrorMessage))
            {
                View.OnLoginError($"Problem with Publisher port: {publisherErrorMessage}");
                return;
            }

            if (!PortIsValid(View.SubscriberPort, false, out var subscriberErrorMessage))
            {
                View.OnLoginError($"Problem with Subscriber port: {subscriberErrorMessage}");
                return;
            }
            
            View.SetLoading(true);
            if (!ValidateThatServerIsAlive())
            {
                View.SetLoading(false);
                View.OnLoginError($"Server seems to be offline.");
                return;
            }
            
            View.SetLoading(false);
            View.OnLoginSuccess();
        }

        private bool ValidateThatServerIsAlive()
        {
            var pubPort = int.Parse(View.PublisherPort);
            var subPort = int.Parse(View.SubscriberPort);
            
            _publisher.Configure(new()
            {
                Username = View.Username,
                Address = _configuration.GetAddress(View.RemoteAddress, pubPort)
            });
            
            _subscriber.Configure(new()
            {
                Address = _configuration.GetAddress(View.RemoteAddress, subPort)
            });
            
            _subscriber.SubscribeToTopic(_configuration.CurrentId.ToString());
            _publisher.Send(new TransferMessage(_configuration.CurrentId.ToString()));

            var response = _subscriber.TryReceiveMessage(TimeSpan.FromSeconds(5));
            
            if (response == null)
            {
                _subscriber.Close();
                _publisher.Close();
                return false;
            }
            
            _subscriber.UnsubscribeToTopic(_configuration.CurrentId.ToString());

            return true;
        }

        private bool PortIsValid(string input, bool testPort, out string message)
        {
            if (!int.TryParse(input, out var subPort))
            {
                message = "Port must be integer";
                return false;   
            }

            if (subPort < 0 || subPort > 65535)
            {
                message = "Port must be between 0 and 65535";
                return false;   
            }

            if (testPort && !_portTester.TestPort(View.RemoteAddress, subPort))
            {
                message = "Could not test the availability of the port";
                return false;   
            }

            message = "";
            return true;
        }
    }
}