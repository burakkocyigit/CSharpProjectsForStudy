using Models;
using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class ClientTcp
    {
        #region Properties
        public int ID { get; set; }
        #endregion
        #region Methods
        public TransmissionData TransmitData()
        {
            try
            {
                TcpClient tc = new TcpClient("127.0.0.1", 8080);
                NetworkStream ns = tc.GetStream();
                TransmissionData transmissionData = new TransmissionData(true) { ID = this.ID };
                string jsonData = JsonConvert.SerializeObject(transmissionData);
                byte[] giden = Encoding.Default.GetBytes(jsonData);
                ns.Write(giden, 0, giden.Length);
                ns.Close();
                tc.Close();
                return transmissionData;
            }
            catch (Exception)
            {
                throw;
                
            }
        } 
        #endregion
    }
}
