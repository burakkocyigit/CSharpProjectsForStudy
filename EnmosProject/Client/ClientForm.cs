using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class ClientForm : Form
    {
        #region Fields
        ClientTcp clientTcp;
        Remoting req;
        #endregion
        #region Methods
        public ClientForm()
        {
            InitializeComponent();
        }
        
        private void ClientForm_Load(object sender, EventArgs e)
        {
            Controls.Add(new DataGridView() { ColumnCount = typeof(TransmissionData).GetProperties().Length, Dock = DockStyle.Fill, Name = "dGrid", AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells });
            RequestID();
            timer.Enabled = true;
        }

        private void RequestID()
        {
            try
            {
                clientTcp = new ClientTcp();
                req = Activator.GetObject(typeof(Remoting), "tcp://localhost:1000/TakeID") as Remoting;
                clientTcp.ID = req.GetID();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                var td = clientTcp.TransmitData();
                ((DataGridView)this.Controls["dGrid"]).Rows.Add(td.TakeParameterForClient(td));
            }
            catch (Exception ex)
            {
                timer.Enabled = false;
                MessageBox.Show(ex.Message);
                return;
            }
        }
        #endregion
    }
}

