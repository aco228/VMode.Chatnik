using System;
using System.Windows.Forms;
using Chatnik.ClientApplication.Core.MessageProcessors;
using Chatnik.ClientApplication.Core.Services;
using Chatnik.ClientApplication.Forms;
using Chatnik.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Chatnik.ClientApplication
{
    public static class Extensions
    {
        public static Form GetView<T>()
            where T : Form
        {
            return Program.ServiceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// TODO: Very bad solution!! but no time to find better one 
        /// </summary>
        public static ChatForm GoToChatForm(this LoginForm form)
        {
            var chatForm = new ChatForm(form,
                Program.ServiceProvider.GetRequiredService<IChatnikPublisherSocket>(),
                Program.ServiceProvider.GetRequiredService<IMessageListener>(),
                Program.ServiceProvider.GetRequiredService<IChatMessageProcessor>(),
                Program.ServiceProvider.GetRequiredService<IHearthbeatService>());
            
            chatForm.Show();
            return chatForm;
        }
        
        public static void InvokeIfRequired<T>(this T c, Action<T> action) where T : Control
        {
            if (c.InvokeRequired)
            {
                c.Invoke(new Action(() => action(c)));
            }
            else
            {
                action(c);
            }
        }
    }
}