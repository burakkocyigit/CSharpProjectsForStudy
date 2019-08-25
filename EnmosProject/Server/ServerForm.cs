using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class ServerForm : Form
    {
        #region Fields
        object lockObj = new object();
        ServerTcp serverTcp;
        #endregion
        #region Methods
        public ServerForm()
        {
            InitializeComponent();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            Controls.Add(new DataGridView() { ColumnCount = typeof(TransmissionData).GetProperties().Length + 1, Dock = DockStyle.Fill, Name = "dGrid", AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells });
            serverTcp = new ServerTcp(8080, 1000, IPAddress.Parse("127.0.0.1"));
            serverTcp.DataReceivedEvent += Tcp_DataReceivedEvent;
            try
            {
                Task.Factory.StartNew(() => serverTcp.ListenClients(), serverTcp.CToken);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Tcp_DataReceivedEvent(TransmissionData transmissionData)
        {
            lock (lockObj)
            {
                try
                {
                    this.Invoke(new Action(() => { ((DataGridView)this.Controls["dGrid"]).Rows.Add(transmissionData.TakeParameterForServer(transmissionData)); }));
                }
                catch (ObjectDisposedException)
                {

                }
            }
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            serverTcp.CTokenSource.Cancel();
            Thread.Sleep(1000);
        }
        #endregion
    }
}
