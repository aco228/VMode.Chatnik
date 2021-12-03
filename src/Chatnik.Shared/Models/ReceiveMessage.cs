using System.Linq;
using Chatnik.Shared.Interfaces;
using NetMQ;

namespace Chatnik.Shared.Models
{
    public class ReceiveMessage : IReceiveMessage
    {
        private readonly NetMQMessage _message;
        
        public string Topic { get; set; }
        public string User { get; set; }
        
        public string[] Frames
        {
            get
            {
                if (!_message.Any())
                    return new string[]{ };

                return _message.Select(x => x.ConvertToString()).ToArray();
            }
        }

        public NetMQMessage NetMQMessage
        {
            get
            {
                var message = new NetMQMessage();
                message.Append(Topic);
                message.Append(User);
                foreach (var frame in Frames)
                    message.Append(frame);

                return message;
            }
        }

        public ReceiveMessage(NetMQMessage message)
        {
            _message = message;
            Topic = _message.Pop().ConvertToString();
            User = _message.Pop().ConvertToString();
        }
    }
}