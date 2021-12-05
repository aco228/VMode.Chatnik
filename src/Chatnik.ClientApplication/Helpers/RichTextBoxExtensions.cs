using System;
using System.Drawing;
using System.Windows.Forms;
using Chatnik.Shared.Models;

namespace Chatnik.ClientApplication
{
    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }

        public static void AddSection(this RichTextBox box, ChatMessage message, Action<RichTextBox> action)
        {
            box.InvokeIfRequired(x =>
            {
                x.AppendText(message.Date.ToLongTimeString());
                action(box);
                x.AppendText(Environment.NewLine);
            });
        }
        
        public static void OnMessage(this RichTextBox box, ChatMessage message)
        {
            box.AppendText($" {message.User} ", Color.Blue);
            box.AppendText(message.Text);
        }
        
        public static void OnUserJoined(this RichTextBox box, ChatMessage message)
        {
            box.AppendText($" USER {message.User} JOINED ", Color.Blue);
        }
        
        public static void OnUserLeft(this RichTextBox box, ChatMessage message)
        {
            box.AppendText($" USER {message.User} LEFT :(  ", Color.Firebrick);
        }
        
        public static void OnServerLostConnection(this RichTextBox box)
        {
            box.AppendText($" SERVER LOST CONNECTION ", Color.Red);
        }
        
        public static void OnServerRegainedConnection(this RichTextBox box)
        {
            box.AppendText($" SERVER REGAINED CONNECTION ", Color.Green);
        }
        
        
    }
}