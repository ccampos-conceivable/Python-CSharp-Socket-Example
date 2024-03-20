using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        ClientSender clientSender;

        public Form1()
        {
            InitializeComponent();

            clientSender = new ClientSender(60000);
            clientSender.OnClientReceived += ClientSender_OnClientReceived;
        }

        private void ClientSender_OnClientReceived(object sender, EventArgs e)
        {
            MessageBox.Show("Data Received");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clientSender.Send("START");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            clientSender.Close();
        }
    }

    public class ClientSender
    {
        private UdpClient udpClient = new UdpClient();
        private int PORT;

        public event EventHandler<EventArgs> OnClientReceived;

        public ClientSender(int port)
        {
            PORT = port;
        }

        public void Send(string text)
        {
            try
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, PORT);
                byte[] bytes = Encoding.ASCII.GetBytes(text);
                udpClient.Send(bytes, bytes.Length, ip);
                udpClient.BeginReceive(Receive, new object());
            }
            catch
            {
            }
        }

        private void Receive(IAsyncResult ar)
        {
            try
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, PORT);
                byte[] bytes = udpClient.EndReceive(ar, ref ip);
                string command = Encoding.ASCII.GetString(bytes);
                if(OnClientReceived != null)
                    OnClientReceived(this, new EventArgs());
            }
            catch
            {
            }
        }

        public void Close()
        {
            udpClient.Close();
        }
    }
}
