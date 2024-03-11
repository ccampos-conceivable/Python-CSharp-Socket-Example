using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using static System.Windows.Forms.AxHost;

namespace Server
{
    public partial class Form1 : Form
    {
        private TcpListener _listener;
        private Thread _listenerThread;
        TcpClient client;
        NetworkStream ns;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void StartListener()
        {
            _listenerThread = new Thread(RunListener);
            _listenerThread.Start();
        }

        private void RunListener()
        {
            try
            {
                _listener = new TcpListener(IPAddress.Any, 50001);
                _listener.Start();

                TcpClient client = _listener.AcceptTcpClient();
                this.Invoke(
                    new Action(
                        () =>
                        {
                            textBox1.Text += string.Format("\nNew connection from {0}", client.Client.RemoteEndPoint) + Environment.NewLine;
                        }
                    )); ;
                ThreadPool.QueueUserWorkItem(ProcessClient, client);
            }
            catch (Exception ex)
            {
                //Log a connection error
            }
        }

        private void ProcessClient(object state)
        {
            client = state as TcpClient;
            ns = client.GetStream();
            byte[] dataReceived = new byte[1024];
            string command = "";

            try
            {
                while (client.Connected)
                {
                    if (ns.DataAvailable)
                    {
                        int dataLen = ns.Read(dataReceived, 0, client.Available);
                        command = ASCIIEncoding.ASCII.GetString(dataReceived, 0, dataLen);
                        this.Invoke(
                            new Action(
                                () =>
                                {
                                    textBox1.Text += "Command received: " + command + Environment.NewLine;
                                }
                            )); ;
                        // Respond acknowledge to Python
                        ns.Write(ASCIIEncoding.ASCII.GetBytes("ACK"), 0, ASCIIEncoding.ASCII.GetBytes("ACK").Length);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log a communication error
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(button1.Text == "Start Listener")
            {
                StartListener();
                textBox1.Text += "Listener Started." + Environment.NewLine;
                button1.Text = "Stop Listener";
            }
            else
            {
                ns.Close();
                client.Close();
                textBox1.Text += "Listener Stopped." + Environment.NewLine;
                button1.Text = "Start Listener";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ns.Write(ASCIIEncoding.ASCII.GetBytes(textBox2.Text), 0, textBox2.Text.Length);
        }
    }
}
