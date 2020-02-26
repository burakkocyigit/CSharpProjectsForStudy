using SocketAsync;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncSocketServer
{
    public partial class Form1 : Form
    {
        SocketServer mServer;
        public Form1()
        {
            InitializeComponent();
            mServer = new SocketServer();
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            mServer.StartListeningForIncomingConnection(new System.Net.IPAddress(new byte[] { 127, 0, 0, 1 }));
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            mServer.SendToAll(textBox1.Text.Trim());
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            mServer.StopServer();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            mServer.StopServer();
        }

    }
}
