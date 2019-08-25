using Models;
using Newtonsoft.Json;
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Server
{
    public class ServerTcp
    {
        #region DelegatesAndEvents
        public delegate void DataReceivedController(TransmissionData transmissionData);
        public event DataReceivedController DataReceivedEvent;
        #endregion
        #region Properties
        public CancellationTokenSource CTokenSource { get; set; }
        public CancellationToken CToken { get; set; }
        #endregion
        #region Fields
        int _portListener;
        int _portChannel;
        TcpListener _tls;
        TcpChannel _tcp;
        #endregion
        #region Methods
        public ServerTcp(int portListener, int portChannel, IPAddress ip = null)
        {
            CTokenSource = new CancellationTokenSource();
            CToken = CTokenSource.Token;

            _portListener = portListener;
            _portChannel = portChannel;
            if (ip != null)
                _tls = new TcpListener(ip, _portListener);            
            else
                _tls = new TcpListener(_portListener);
            _tcp = new TcpChannel(_portChannel);
            ChannelServices.RegisterChannel(_tcp, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(Remoting), "TakeID", WellKnownObjectMode.SingleCall);
        }
        public void ListenClients()
        {
            try
            {
                _tls.Start();
                while (true)
                {
                    Socket soc = _tls.AcceptSocket();

                    NetworkStream ns = new NetworkStream(soc);
                    List<byte> received = new List<byte>();
                    int data = ns.ReadByte();
                    while (data != -1)
                    {
                        received.Add((byte)data);
                        data = ns.ReadByte();
                    }
                    ns.Close();

                    byte[] buffer = received.ToArray();
                    string readData = Encoding.Default.GetString(buffer);
                    TransmissionData td = JsonConvert.DeserializeObject<TransmissionData>(readData);
                    if (CToken.IsCancellationRequested)
                    {
                        td = null;
                        break;
                    }
                    if (td != null)
                    {
                        Task.Run(() => DataReceivedEvent(td));
                    }
                    soc.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
