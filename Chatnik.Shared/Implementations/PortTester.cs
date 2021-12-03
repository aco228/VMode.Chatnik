using System.Net;
using System.Net.Sockets;
using Chatnik.Shared.Interfaces;

namespace Chatnik.Shared.Implementations
{
    public class PortTester : IPortTester
    {
        public bool TestPort(string remoteAddress, int port)
        {
            var tcpListener = default(TcpListener);
            var result = false;

            try
            {
                var ipAddress = Dns.GetHostEntry(remoteAddress).AddressList[0];

                tcpListener = new TcpListener(ipAddress, port);
                tcpListener.Start();

                result = true;
            }
            catch (SocketException ex)
            {
                result = false;
            }
            finally
            {
                if (tcpListener != null)
                    tcpListener.Stop();
            }

            return result;
        }
    }
}