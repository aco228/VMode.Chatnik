using System.Threading.Tasks;
using Chatnik.Shared.Helpers;
using Chatnik.Shared.Interfaces;

namespace Chatnik.Shared.Implementations
{
    public abstract class MessageProcessorBase<T> : IMessageProcessor
        where T : ITransferMessage
    {
        
        /// <summary>
        /// TODO: explain
        /// </summary>
        /// <param name="message"></param>
        protected abstract void Process(T message);

        public virtual void ProcessMessage(IReceiveMessage message)
            => Task.Run(() => Process(message.Convert<T>()));
    }
}