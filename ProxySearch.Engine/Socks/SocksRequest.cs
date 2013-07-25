using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ProxySearch.Engine.Socks
{
    public class SocksRequest
    {
        public async Task V4(NetworkStream stream, IPEndPoint remoteEP)
        {
            try
            {
                byte[] data = new byte[9] { 4, 1, 0, 0, 0, 0, 0, 0, 0 };

                Array.Copy(PortToBytes((ushort)remoteEP.Port), 0, data, 2, 2);
                Array.Copy(remoteEP.Address.GetAddressBytes(), 0, data, 4, 4);

                await stream.WriteAsync(data, 0, data.Length);

                if ((await ReadBytes(stream, 8))[1] != 90)
                {
                    throw new SocksRequestFailedException("Negotiation failed.");
                }
            }
            catch
            {
                throw new SocksRequestFailedException();
            }
        }

        public async Task V5(NetworkStream stream, IPEndPoint remoteEP)
        {
            await AuthenticateV5(stream);

            byte[] data = new byte[10] { 5, 1, 0, 1, 0, 0, 0, 0, 0, 0 };

            Array.Copy(remoteEP.Address.GetAddressBytes(), 0, data, 4, 4);
            Array.Copy(PortToBytes((ushort)remoteEP.Port), 0, data, 8, 2);

            stream.Write(data, 0, data.Length);
            byte[] buffer = await ReadBytes(stream, 4);
            if (buffer[1] != 0)
            {
                throw new SocksRequestFailedException();
            }
            switch (buffer[3])
            {
                case 1:
                    await ReadBytes(stream, 6);
                    break;
                case 3:
                    await ReadBytes(stream, (await ReadBytes(stream, 1)).Single() + 2);
                    break;
                case 4:
                    await ReadBytes(stream, 18);
                    break;
                default:
                    throw new SocksRequestFailedException();
            }
        }

        private async Task AuthenticateV5(NetworkStream stream)
        {
            stream.Write(new byte[] { 5, 2, 0, 2 }, 0, 4);

            switch ((await ReadBytes(stream, 2)).Last())
            {
                case 0:
                    break;
                case 2:
                    await stream.WriteAsync(new byte[] { 1, 0, 0 }, 0, 3);
                    byte[] buffer = new byte[2];
                    int received = 0;
                    while (received != 2)
                    {
                        received += await stream.ReadAsync(buffer, received, 2 - received);
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

        private async Task<byte[]> ReadBytes(NetworkStream stream, int count)
        {
            if (count <= 0)
                throw new ArgumentException();
            byte[] buffer = new byte[count];
            int received = 0;
            while (received != count)
            {
                int result = await stream.ReadAsync(buffer, received, count - received);

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
