using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net.NetworkInformation;

namespace Client
{
    public partial class Form1 : Form
    {
        NetworkInterface[] localInterfaces;
        List<IPAddress> localAdaptersAddresse;
        IPEndPoint localEndPoint;
        int localPort = 0;

        IPAddress unitAddress;
        IPEndPoint unitEndPoint;

        Socket connection;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Search for local adapters to use for the connection.
            localInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            localAdaptersAddresse = new List<IPAddress>();
            foreach (var adapter in localInterfaces)
            {
                var ipProps = adapter.GetIPProperties();

                foreach (var ip in ipProps.UnicastAddresses)
                {
                    if ((ip.Address.AddressFamily == AddressFamily.InterNetwork))
                    {
                        localAdaptersAddresse.Add(ip.Address);
                        comboBox1.Items.Add(adapter.Name.ToString() + " / " + ip.Address.ToString());
                        Console.Out.WriteLine(adapter.Name.ToString() + " / " + ip.Address.ToString());
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Connect")
            {
                //Local
                localEndPoint = new IPEndPoint(localAdaptersAddresse[comboBox1.SelectedIndex], localPort);

                //Unit
                string[] sAddress = textBox1.Text.Split('.');
                byte[] bAddress = new byte[] { byte.Parse(sAddress[0]),
                                           byte.Parse(sAddress[1]),
                                           byte.Parse(sAddress[2]),
                                           byte.Parse(sAddress[3])};
                unitAddress = new IPAddress(bAddress);
                unitEndPoint = new IPEndPoint(unitAddress, int.Parse(textBox2.Text));

                connection = new Socket(unitEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                connection.Bind(localEndPoint);
                connection.Connect(unitEndPoint);
                button1.Text = "Disconnect";
            }
            else
            {
                connection.Disconnect(true);
                button1.Text = "Connect";
            }
        }
    }
}
