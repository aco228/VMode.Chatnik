using NetMQ;

namespace Chatnik.Shared.Interfaces
{
    public interface IReceiveMessage : IMessage
    {
        public string Topic { get; set; }
        public string User { get; set; }
        public string[] Frames { get; }
        
        /// <summary>
        /// Recreate original message
        /// </summary>
        public NetMQMessage NetMQMessage { get; } 
    }
}