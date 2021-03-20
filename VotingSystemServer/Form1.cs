using SimpleTcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VotingSystemServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        SimpleTcpServer server;

        private void btnStart_Click(object sender, EventArgs e)
        {
            server.Start();
            txtInfo.Text += $"Starting...{Environment.NewLine}";
            btnStart.Enabled = false;
            btnSend.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            server = new SimpleTcpServer(txtIP.Text);
            server.Events.ClientConnected += Events_ClientConnected;
            server.Events.ClientDisconnected += Events_ClientDisconnected;
            server.Events.DataReceived += Events_DataReceived;
        }

        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort}:{Encoding.UTF8.GetString(e.Data)}{Environment.NewLine}";
            });
        }
        private void Events_ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort} disconnected.{Environment.NewLine}";
                lstClientIP.Items.Remove(e.IpPort);
            });

          
        }

        private void Events_ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort} connected.{Environment.NewLine}";
                lstClientIP.Items.Add(e.IpPort);
            });
            
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            
            //Ipsd= lstClientIP.SelectedItem;
            foreach (var item in lstClientIP.SelectedItems)
            {
                if (server.IsListening)
                {
                    if (!string.IsNullOrEmpty(txtMessage.Text))//check message &select client io from listbox  && lstClientIP.SelectedItem != null
                    {
                        server.Send(item.ToString(), txtMessage.Text);
                        txtInfo.Text += $"Server: {txtMessage.Text}{Environment.NewLine}";
                        txtMessage.Text = string.Empty;
                    }
                }
            }
            //if (server.IsListening)
            //{
            //    if (!string.IsNullOrEmpty(txtMessage.Text)&&lstClientIP.SelectedItem!=null)//check message &select client io from listbox
            //    {
            //        server.Send(lstClientIP.SelectedItem.ToString(), txtMessage.Text);
            //        txtInfo.Text += $"Server: {txtMessage.Text}{Environment.NewLine}";
            //        txtMessage.Text = string.Empty;
            //    }
            //}
        }
    }
}
