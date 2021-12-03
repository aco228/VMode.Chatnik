namespace Chatnik.Shared.Interfaces
{
    public interface ITransferMessage : IMessage
    {
        public string Topic { get; set; }
        public string User { get; set; }
    }
}