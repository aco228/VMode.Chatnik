using System;

namespace Chatnik.Shared.Models
{
    public class ChatMessage : TransferMessage
    {
        public string Text { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public ChatMessageType Type { get; set; } = ChatMessageType.Message;

        public ChatMessage(string topic) : base(topic) { }
        public ChatMessage() : base(GlobalConstants.MainChannel) { }
    }
}