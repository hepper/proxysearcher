using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Properties;

namespace ProxySearch.Engine.Socks
{
    public class SocksRequest
    {
        public async Task V4(NetworkStream stream, Uri uri, CancellationToken cancellationToken)
        {
            try
            {
                byte[] data = new byte[9] { 4, 1, 0, 0, 0, 0, 0, 0, 0 };

                Array.Copy(PortToBytes((ushort) uri.Port), 0, data, 2, 2);
                Array.Copy(GetIpAddress(uri.Host).GetAddressBytes(), 0, data, 4, 4);

                await stream.WriteAsync(data, 0, data.Length, cancellationToken);

                if ((await ReadBytes(stream, 8, cancellationToken))[1] != 90)
                {
                    throw new SocksRequestFailedException("Negotiation failed.");
                }
            }
            catch
            {
                throw new SocksRequestFailedException();
            }
        }

        public async Task V5(NetworkStream stream, Uri uri, CancellationToken cancellationToken)
        {
            await AuthenticateV5(stream, cancellationToken);

            byte[] data = new byte[10] { 5, 1, 0, 1, 0, 0, 0, 0, 0, 0 };

            Array.Copy(GetIpAddress(uri.Host).GetAddressBytes(), 0, data, 4, 4);
            Array.Copy(PortToBytes((ushort)uri.Port), 0, data, 8, 2);

            await stream.WriteAsync(data, 0, data.Length, cancellationToken);
            byte[] buffer = await ReadBytes(stream, 4, cancellationToken);
            if (buffer[1] != 0)
            {
                throw new SocksRequestFailedException();
            }
            switch (buffer[3])
            {
                case 1:
                    await ReadBytes(stream, 6, cancellationToken);
                    break;
                case 3:
                    await ReadBytes(stream, (await ReadBytes(stream, 1, cancellationToken)).Single() + 2, cancellationToken);
                    break;
                case 4:
                    await ReadBytes(stream, 18, cancellationToken);
                    break;
                default:
                    throw new SocksRequestFailedException();
            }
        }

        private async Task AuthenticateV5(NetworkStream stream, CancellationToken cancellationToken)
        {
            await stream.WriteAsync(new byte[] { 5, 2, 0, 2 }, 0, 4, cancellationToken);

            switch ((await ReadBytes(stream, 2, cancellationToken)).Last())
            {
                case 0:
                    break;
                case 2:
                    await stream.WriteAsync(new byte[] { 1, 0, 0 }, 0, 3, cancellationToken);
                    byte[] buffer = new byte[2];
                    int received = 0;
                    while (received != 2)
                    {
                        received += await stream.ReadAsync(buffer, received, 2 - received, cancellationToken);
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

        private async Task<byte[]> ReadBytes(NetworkStream stream, int count, CancellationToken cancellationToken)
        {
            if (count <= 0)
                throw new ArgumentException();
            byte[] buffer = new byte[count];
            int received = 0;
            while (received != count)
            {
                int result = await stream.ReadAsync(buffer, received, count - received, cancellationToken);

                if (result == 0)
                {
                    throw new SocketException(10060);
                }

                received += result;
            }

            return buffer;
        }

        private IPAddress GetIpAddress(string host)
        {
            IPAddress ipAddress;

            if (IPAddress.TryParse(host, out ipAddress))
            {
                return ipAddress;
            }

            try
            {
                return Dns.GetHostEntry(host).AddressList[0];
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(string.Format(Resources.UnableToResolveProxyHostnameFormat, host), e);
            }
        }
    }
}
