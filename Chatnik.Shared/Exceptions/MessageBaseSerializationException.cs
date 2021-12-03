using System;

namespace Chatnik.Shared.Exceptions
{
    public class MessageBaseSerializationException : Exception
    {
        public int Position { get; set; }
        public string ProperyName { get; set; }
        
        public MessageBaseSerializationException(int position, string propertyName)
        {
            Position = position;
            ProperyName = propertyName;
        }
    }
}