using System;
using System.Windows.Forms;
using Chatnik.ClientApplication.Presenters;
using Chatnik.ClientApplication.Views;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;

namespace Chatnik.ClientApplication.Forms
{
    public partial class LoginForm : Form, ILoginView
    {
        private readonly ILoginFormPresenter _presenter;
        
        public string RemoteAddress { get => textBoxRemoteAddress.Text; set => textBoxRemoteAddress.Text = value; }
        public string SubscriberPort { get => textBoxSubscriberPort.Text; set => textBoxSubscriberPort.Text = value; }
        public string PublisherPort { get => textBoxPublisherPort.Text; set => textBoxPublisherPort.Text = value; }
        public string Username { get => textBoxUsername.Text; set => textBoxUsername.Text = value; }
        
        public LoginForm(
            DefaultApplicationConfiguration configuration,
            IChatnikPublisherSocket publisherSocket,
            IChatnikSubscriberSocket subscriberSocket,
            IPortTester portTester)
        {
            InitializeComponent();
            _presenter = new LoginFormPresenter(configuration, publisherSocket, subscriberSocket, portTester, this);
        }

        private void LoginForm_Load(object sender, EventArgs e)
            => _presenter.OnFormLoaded();

        private void buttonLogin_Click(object sender, EventArgs e)
            => _presenter.OnLoginButtonClick();

        public void SetLoading(bool isLoading)
        {
            buttonLogin.Enabled = !isLoading;
        }

        public void OnLoginError(string errorMessage)
        {
            MessageBox.Show(errorMessage);
        }

        public void OnLoginSuccess()
        {
            this.GoToChatForm();
            Hide();
        }
    }
}
