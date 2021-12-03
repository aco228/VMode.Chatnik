using System;
using System.Windows.Forms;
using Chatnik.ClientApplication.Core.MessageProcessors;
using Chatnik.ClientApplication.Core.Services;
using Chatnik.ClientApplication.Presenters;
using Chatnik.ClientApplication.Views;
using Chatnik.Shared.Interfaces;
using Chatnik.Shared.Models;

namespace Chatnik.ClientApplication.Forms
{
    public partial class ChatForm : Form, IChatView
    {
        private readonly LoginForm _loginForm;
        private readonly IChatFormPresenter _presenter;
        
        public string ChatMessage { get => textBoxChatBox.Text; set => textBoxChatBox.Text = value; }

        public ChatForm(
            LoginForm loginForm,
            IChatnikPublisherSocket publisherSocket,
            IMessageListener messageListener,
            IChatMessageProcessor chatMessageProcessor,
            IHearthbeatService hearthbeatService)
        {
            _loginForm = loginForm;
            _presenter = new ChatFormPresenter(publisherSocket, messageListener, chatMessageProcessor, hearthbeatService, this);
            InitializeComponent();
        }

        private void ChatForm_Load(object sender, EventArgs e)
            => _presenter.OnFormLoaded();

        private void buttonSend_Click(object sender, EventArgs e)
            => _presenter.OnSubmitMessageClick();

        public void OnChatMessageReceived(ChatMessage message)
        {
            richTextBox.AddSection(message, x =>
            {
                if(message.Type == ChatMessageType.Message)
                    x.OnMessage(message);
                
                if(message.Type == ChatMessageType.ServerLostConnection)
                    x.OnServerLostConnection();
                
                if(message.Type == ChatMessageType.ServerRegainedConnection)
                    x.OnServerRegainedConnection();
                
                if(message.Type == ChatMessageType.UserJoined)
                    x.OnUserJoined(message);
                
                if(message.Type == ChatMessageType.UserLeft)
                    x.OnUserLeft(message);
            });
        }

        public void OnServerNotResponding()
        {
            MessageBox.Show("Lost connection to the server.");
            buttonSend.InvokeIfRequired(x => x.Enabled = false);
            textBoxChatBox.InvokeIfRequired(x => x.Enabled = false);
        }

        public void OnServerStartResponding()
        {
            buttonSend.InvokeIfRequired(x => x.Enabled = true);
            textBoxChatBox.InvokeIfRequired(x => x.Enabled = true);
        }

        private void textBoxChatBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _presenter.OnSubmitMessageClick();
                e.SuppressKeyPress = true;
            }
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _presenter.OnClosing();
            _loginForm.Show();
        }
    }
}
