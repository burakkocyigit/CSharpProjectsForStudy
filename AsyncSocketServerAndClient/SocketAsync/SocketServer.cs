using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketAsync
{
    public class SocketServer
    {
        IPAddress mIP;
        int mPort;
        TcpListener mTCPListener;
        List<TcpClient> mClients;
        public bool KeepRunning { get; set; }
        public SocketServer()
        {
            mClients = new List<TcpClient>();
        }
        public async void StartListeningForIncomingConnection(IPAddress ipaddr = null, int port = 23000)
        {
            if (ipaddr == null)
            {
                ipaddr = IPAddress.Any;
            }
            if (port >= 0)
            {
                port = 23000;
            }
            mIP = ipaddr;
            mPort = port;
            Debug.WriteLine(string.Format("IP Address: {0} - Port: {1}", mIP, mPort));
            mTCPListener = new TcpListener(mIP, mPort);
            try
            {
                mTCPListener.Start();
                KeepRunning = true;
                while (KeepRunning)
                {
                    var returnByAccept = await mTCPListener.AcceptTcpClientAsync();
                    mClients.Add(returnByAccept);
                    Debug.WriteLine("Client connected successfully, count: {0} - {1}", mClients.Count, returnByAccept.Client.RemoteEndPoint);
                    TakeCareOfTCPClient(returnByAccept);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public void StopServer()   
        {
            try
            {
                if (mTCPListener!=null)
                {
                    mTCPListener.Stop();
                }
                foreach (TcpClient item in mClients)
                {
                    item.Close();
                }
                mClients.Clear();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private async void TakeCareOfTCPClient(TcpClient paramClient)
        {
            NetworkStream stream = null;
            StreamReader reader = null;
            try
            {
                stream = paramClient.GetStream();
                reader = new StreamReader(stream);
                char[] buff = new char[64];
                while (KeepRunning)
                {
                    Debug.WriteLine("***Ready to read");
                    int nRet = await reader.ReadAsync(buff, 0, buff.Length);
                    Debug.WriteLine("Returned: " + nRet);
                    if (nRet == 0)
                    {
                        RemoveClient(paramClient);
                        Debug.WriteLine("Socket disconnected");
                        break;
                    }
                    string receivedText = new string(buff);
                    Debug.WriteLine("***RECEIVED: " + receivedText);
                    Array.Clear(buff, 0, buff.Length);
                }
            }
            catch (Exception ex)
            {
                RemoveClient(paramClient);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        private void RemoveClient(TcpClient paramClietnt)
        {
            if (mClients.Contains(paramClietnt))
            {
                mClients.Remove(paramClietnt);
                Debug.WriteLine(string.Format("Client removed, count: {0}", mClients.Count));
            }
        }
        public async void SendToAll(string leMessage)
        {
            if (string.IsNullOrEmpty(leMessage))
            {
                return;
            }
            try
            {
                byte[] buffMessage = Encoding.ASCII.GetBytes(leMessage);
                foreach (TcpClient item in mClients)
                {
                    item.GetStream().WriteAsync(buffMessage, 0, buffMessage.Length);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
