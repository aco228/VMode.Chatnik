namespace Chatnik.Shared.Interfaces
{
    public interface IPortTester
    {
        /// <summary>
        /// Verify that port is avaliable at remote address
        /// </summary>
        /// <param name="remoteAddress">Remote address in string (ex. 127.0.0.1)</param>
        /// <param name="port">Port number</param>
        public bool TestPort(string remoteAddress, int port);
    }
}