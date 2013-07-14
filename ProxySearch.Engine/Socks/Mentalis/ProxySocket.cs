/*
    Copyright © 2002, The KPD-Team
    All rights reserved.
    http://www.mentalis.org/

  Redistribution and use in source and binary forms, with or without
  modification, are permitted provided that the following conditions
  are met:

    - Redistributions of source code must retain the above copyright
       notice, this list of conditions and the following disclaimer. 

    - Neither the name of the KPD-Team, nor the names of its contributors
       may be used to endorse or promote products derived from this
       software without specific prior written permission. 

  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
  FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL
  THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
  (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
  SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
  HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
  STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
  OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Net;
using System.Net.Sockets;

// Implements a number of classes to allow Sockets to connect trough a firewall.
namespace ProxySearch.Engine.Socks.Mentalis
{
    /// <summary>
    /// Implements a Socket class that can connect trough a SOCKS proxy server.
    /// </summary>
    /// <remarks>This class implements SOCKS4[A] and SOCKS5.<br>It does not, however, implement the BIND commands, so you cannot .</br></remarks>
    public class ProxySocket : Socket
    {
        private AsyncCallback CallBack = null;

        /// <summary>
        /// Initializes a new instance of the ProxySocket class.
        /// </summary>
        /// <param name="addressFamily">One of the AddressFamily values.</param>
        /// <param name="socketType">One of the SocketType values.</param>
        /// <param name="protocolType">One of the ProtocolType values.</param>
        /// <exception cref="SocketException">The combination of addressFamily, socketType, and protocolType results in an invalid socket.</exception>
        public ProxySocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) : this(addressFamily, socketType, protocolType, "") { }
        /// <summary>
        /// Initializes a new instance of the ProxySocket class.
        /// </summary>
        /// <param name="addressFamily">One of the AddressFamily values.</param>
        /// <param name="socketType">One of the SocketType values.</param>
        /// <param name="protocolType">One of the ProtocolType values.</param>
        /// <param name="proxyUsername">The username to use when authenticating with the proxy server.</param>
        /// <exception cref="SocketException">The combination of addressFamily, socketType, and protocolType results in an invalid socket.</exception>
        /// <exception cref="ArgumentNullException"><c>proxyUsername</c> is null.</exception>
        public ProxySocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType, string proxyUsername) : this(addressFamily, socketType, protocolType, proxyUsername, "") { }
        /// <summary>
        /// Initializes a new instance of the ProxySocket class.
        /// </summary>
        /// <param name="addressFamily">One of the AddressFamily values.</param>
        /// <param name="socketType">One of the SocketType values.</param>
        /// <param name="protocolType">One of the ProtocolType values.</param>
        /// <param name="proxyUsername">The username to use when authenticating with the proxy server.</param>
        /// <param name="proxyPassword">The password to use when authenticating with the proxy server.</param>
        /// <exception cref="SocketException">The combination of addressFamily, socketType, and protocolType results in an invalid socket.</exception>
        /// <exception cref="ArgumentNullException"><c>proxyUsername</c> -or- <c>proxyPassword</c> is null.</exception>
        public ProxySocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType, string proxyUsername, string proxyPassword)
            : base(addressFamily, socketType, protocolType)
        {
            ProxyUser = proxyUsername;
            ProxyPass = proxyPassword;
            ToThrow = new InvalidOperationException();
        }

        /// <summary>
        /// Establishes a connection to a remote device.
        /// </summary>
        /// <param name="remoteEP">An EndPoint that represents the remote device.</param>
        /// <exception cref="ArgumentNullException">The remoteEP parameter is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="SocketException">An operating system error occurs while accessing the Socket.</exception>
        /// <exception cref="ObjectDisposedException">The Socket has been closed.</exception>
        /// <exception cref="ProxyException">An error occured while talking to the proxy server.</exception>
        public new void Connect(EndPoint remoteEP)
        {
            if (remoteEP == null)
                throw new ArgumentNullException("<remoteEP> cannot be null.");
            if (this.ProtocolType != ProtocolType.Tcp || ProxyType == ProxyTypes.None || ProxyEndPoint == null)
                base.Connect(remoteEP);
            else
            {
                base.Connect(ProxyEndPoint);
                if (ProxyType == ProxyTypes.Socks4)
                    (new Socks4Handler(this, ProxyUser)).Negotiate((IPEndPoint)remoteEP);
                else if (ProxyType == ProxyTypes.Socks5)
                    (new Socks5Handler(this, ProxyUser, ProxyPass)).Negotiate((IPEndPoint)remoteEP);
            }
        }

        /// <summary>
        /// Establishes a connection to a remote device.
        /// </summary>
        /// <param name="host">The remote host to connect to.</param>
        /// <param name="port">The remote port to connect to.</param>
        /// <exception cref="ArgumentNullException">The host parameter is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The port parameter is invalid.</exception>
        /// <exception cref="SocketException">An operating system error occurs while accessing the Socket.</exception>
        /// <exception cref="ObjectDisposedException">The Socket has been closed.</exception>
        /// <exception cref="ProxyException">An error occured while talking to the proxy server.</exception>
        /// <remarks>If you use this method with a SOCKS4 server, it will let the server resolve the hostname. Not all SOCKS4 servers support this 'remote DNS' though.</remarks>
        public new void Connect(string host, int port)
        {
            if (host == null)
                throw new ArgumentNullException("<host> cannot be null.");
            if (port <= 0 || port > 65535)
                throw new ArgumentException("Invalid port.");
            if (this.ProtocolType != ProtocolType.Tcp || ProxyType == ProxyTypes.None || ProxyEndPoint == null)
                base.Connect(new IPEndPoint(Dns.GetHostEntry(host).AddressList[0], port));
            else
            {
                base.Connect(ProxyEndPoint);
                if (ProxyType == ProxyTypes.Socks4)
                    (new Socks4Handler(this, ProxyUser)).Negotiate(host, port);
                else if (ProxyType == ProxyTypes.Socks5)
                    (new Socks5Handler(this, ProxyUser, ProxyPass)).Negotiate(host, port);
            }
        }

        /// <summary>
        /// Begins an asynchronous request for a connection to a network device.
        /// </summary>
        /// <param name="remoteEP">An EndPoint that represents the remote device.</param>
        /// <param name="callback">The AsyncCallback delegate.</param>
        /// <param name="state">An object that contains state information for this request.</param>
        /// <returns>An IAsyncResult that references the asynchronous connection.</returns>
        /// <exception cref="ArgumentNullException">The remoteEP parameter is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="SocketException">An operating system error occurs while creating the Socket.</exception>
        /// <exception cref="ObjectDisposedException">The Socket has been closed.</exception>
        public new IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
        {
            if (remoteEP == null || callback == null)
                throw new ArgumentNullException();
            if (this.ProtocolType != ProtocolType.Tcp || ProxyType == ProxyTypes.None || ProxyEndPoint == null)
            {
                return base.BeginConnect(remoteEP, callback, state);
            }
            else
            {
                CallBack = callback;
                if (ProxyType == ProxyTypes.Socks4)
                {
                    AsyncResult = (new Socks4Handler(this, ProxyUser)).BeginNegotiate((IPEndPoint)remoteEP, new HandShakeComplete(this.OnHandShakeComplete), ProxyEndPoint);
                    return AsyncResult;
                }
                else if (ProxyType == ProxyTypes.Socks5)
                {
                    AsyncResult = (new Socks5Handler(this, ProxyUser, ProxyPass)).BeginNegotiate((IPEndPoint)remoteEP, new HandShakeComplete(this.OnHandShakeComplete), ProxyEndPoint);
                    return AsyncResult;
                }
                return null;
            }
        }
        /// <summary>
        /// Begins an asynchronous request for a connection to a network device.
        /// </summary>
        /// <param name="host">The host to connect to.</param>
        /// <param name="port">The port on the remote host to connect to.</param>
        /// <param name="callback">The AsyncCallback delegate.</param>
        /// <param name="state">An object that contains state information for this request.</param>
        /// <returns>An IAsyncResult that references the asynchronous connection.</returns>
        /// <exception cref="ArgumentNullException">The host parameter is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The port parameter is invalid.</exception>
        /// <exception cref="SocketException">An operating system error occurs while creating the Socket.</exception>
        /// <exception cref="ObjectDisposedException">The Socket has been closed.</exception>
        public new IAsyncResult BeginConnect(string host, int port, AsyncCallback callback, object state)
        {
            if (host == null || callback == null)
                throw new ArgumentNullException();
            if (port <= 0 || port > 65535)
                throw new ArgumentException();
            CallBack = callback;
            if (this.ProtocolType != ProtocolType.Tcp || ProxyType == ProxyTypes.None || ProxyEndPoint == null)
            {
                RemotePort = port;
                AsyncResult = BeginDns(host, new HandShakeComplete(this.OnHandShakeComplete));
                return AsyncResult;
            }
            else
            {
                if (ProxyType == ProxyTypes.Socks4)
                {
                    AsyncResult = (new Socks4Handler(this, ProxyUser)).BeginNegotiate(host, port, new HandShakeComplete(this.OnHandShakeComplete), ProxyEndPoint);
                    return AsyncResult;
                }
                else if (ProxyType == ProxyTypes.Socks5)
                {
                    AsyncResult = (new Socks5Handler(this, ProxyUser, ProxyPass)).BeginNegotiate(host, port, new HandShakeComplete(this.OnHandShakeComplete), ProxyEndPoint);
                    return AsyncResult;
                }
                return null;
            }
        }
        /// <summary>
        /// Ends a pending asynchronous connection request.
        /// </summary>
        /// <param name="asyncResult">Stores state information for this asynchronous operation as well as any user-defined data.</param>
        /// <exception cref="ArgumentNullException">The asyncResult parameter is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The asyncResult parameter was not returned by a call to the BeginConnect method.</exception>
        /// <exception cref="SocketException">An operating system error occurs while accessing the Socket.</exception>
        /// <exception cref="ObjectDisposedException">The Socket has been closed.</exception>
        /// <exception cref="InvalidOperationException">EndConnect was previously called for the asynchronous connection.</exception>
        /// <exception cref="ProxyException">The proxy server refused the connection.</exception>
        public new void EndConnect(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentNullException();
            if (!asyncResult.IsCompleted)
                throw new ArgumentException();
            if (ToThrow != null)
                throw ToThrow;
            return;
        }
        /// <summary>
        /// Begins an asynchronous request to resolve a DNS host name or IP address in dotted-quad notation to an IPAddress instance.
        /// </summary>
        /// <param name="host">The host to resolve.</param>
        /// <param name="callback">The method to call when the hostname has been resolved.</param>
        /// <returns>An IAsyncResult instance that references the asynchronous request.</returns>
        /// <exception cref="SocketException">There was an error while trying to resolve the host.</exception>
        internal IAsyncProxyResult BeginDns(string host, HandShakeComplete callback)
        {
            try
            {
                Dns.BeginGetHostEntry(host, new AsyncCallback(this.OnResolved), this);
                return new IAsyncProxyResult();
            }
            catch
            {
                throw new SocketException();
            }
        }
        /// <summary>
        /// Called when the specified hostname has been resolved.
        /// </summary>
        /// <param name="asyncResult">The result of the asynchronous operation.</param>
        private void OnResolved(IAsyncResult asyncResult)
        {
            try
            {
                IPHostEntry dns = Dns.EndGetHostEntry(asyncResult);
                base.BeginConnect(new IPEndPoint(dns.AddressList[0], RemotePort), new AsyncCallback(this.OnConnect), State);
            }
            catch (Exception e)
            {
                OnHandShakeComplete(e);
            }
        }
        /// <summary>
        /// Called when the Socket is connected to the remote host.
        /// </summary>
        /// <param name="asyncResult">The result of the asynchronous operation.</param>
        private void OnConnect(IAsyncResult asyncResult)
        {
            try
            {
                base.EndConnect(asyncResult);
                OnHandShakeComplete(null);
            }
            catch (Exception e)
            {
                OnHandShakeComplete(e);
            }
        }
        /// <summary>
        /// Called when the Socket has finished talking to the proxy server and is ready to relay data.
        /// </summary>
        /// <param name="error">The error to throw when the EndConnect method is called.</param>
        private void OnHandShakeComplete(Exception error)
        {
            if (error != null)
                this.Close();
            ToThrow = error;
            AsyncResult.Reset();
            if (CallBack != null)
                CallBack(AsyncResult);
        }
        /// <summary>
        /// Gets or sets the EndPoint of the proxy server.
        /// </summary>
        /// <value>An IPEndPoint object that holds the IP address and the port of the proxy server.</value>
        public IPEndPoint ProxyEndPoint
        {
            get;
            set;
        }

        public ProxyTypes ProxyType
        {
            get;
            set;
        }

        private object State
        {
            get;
            set;
        }

        private string m_ProxyUser = null;
        public string ProxyUser
        {
            get
            {
                return m_ProxyUser;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                m_ProxyUser = value;
            }
        }

        private string m_ProxyPass = null;
        public string ProxyPass
        {
            get
            {
                return m_ProxyPass;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                m_ProxyPass = value;
            }
        }

        private IAsyncProxyResult AsyncResult
        {
            get;
            set;
        }

        private Exception ToThrow
        {
            get;
            set;
        }

        private int RemotePort
        {
            get;
            set;
        } 
    }
}