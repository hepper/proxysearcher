using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace ProxySearch.Engine.Socks
{
    public class SocksRequest
    {
        public void V4(Socket socket, IPEndPoint remoteEP)
        {
            byte[] data = new byte[9] { 4, 1, 0, 0, 0, 0, 0, 0, 0 };

            Array.Copy(PortToBytes((ushort)remoteEP.Port), 0, data, 2, 2);
            Array.Copy(remoteEP.Address.GetAddressBytes(), 0, data, 4, 4);

            //data[8] = 0;

            socket.Send(data);
            if (ReadBytes(socket, 8)[1] != 90)
            {
                throw new SocksRequestFailedException("Negotiation failed.");
            }
        }

        public void V5(Socket socket, IPEndPoint remoteEP)
        {
            AuthenticateV5(socket);

            byte[] data = new byte[10] { 5, 1, 0, 1, 0, 0, 0, 0, 0, 0 };

            Array.Copy(remoteEP.Address.GetAddressBytes(), 0, data, 4, 4);
            Array.Copy(PortToBytes((ushort)remoteEP.Port), 0, data, 8, 2);

            socket.Send(data);
            byte[] buffer = ReadBytes(socket, 4);
            if (buffer[1] != 0)
            {
                throw new SocksRequestFailedException();
            }
            switch (buffer[3])
            {
                case 1:
                    ReadBytes(socket, 6);
                    break;
                case 3:
                    ReadBytes(socket, ReadBytes(socket, 1).Single() + 2);
                    break;
                case 4:
                    ReadBytes(socket, 18);
                    break;
                default:
                    throw new SocksRequestFailedException();
            }
        }

        private void AuthenticateV5(Socket socket)
        {
            socket.Send(new byte[] { 5, 2, 0, 2 });

            switch (ReadBytes(socket, 2).Last())
            {
                case 0:
                    break;
                case 2:
                    socket.Send(new byte[] { 1, 0, 0 });
                    byte[] buffer = new byte[2];
                    int received = 0;
                    while (received != 2)
                    {
                        received += socket.Receive(buffer, received, 2 - received, SocketFlags.None);
                    }

                    if (buffer[1] != 0)
                    {
                        throw new SocksRequestFailedException("Authentication failed");
                    }
                    break;
                case 255:
                    throw new SocksRequestFailedException("No authentication method accepted.");
                default:
                    throw new SocksRequestFailedException();
            }
        }

        private byte[] PortToBytes(ushort port)
        {
            return BitConverter.GetBytes(port).Reverse().ToArray();
        }

        private byte[] ReadBytes(Socket socket, int count)
        {
            if (count <= 0)
                throw new ArgumentException();
            byte[] buffer = new byte[count];
            int received = 0;
            while (received != count)
            {
                int result = socket.Receive(buffer, received, count - received, SocketFlags.None);

                if (result == 0)
                {
                    throw new SocketException(10060);
                }

                received += result;
            }
            return buffer;
        }
    }
}
