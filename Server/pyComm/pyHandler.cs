using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;


namespace pyComm
{
    public class pyHandler
    {
        #region CLASS_VARIABLES

        private UdpClient udp;
        private IPEndPoint ipClient;
        private int PORT = 60000;

        #endregion

        #region PUBLIC_EVENTS

        public event EventHandler<EventArgs> OnStart;

        #endregion

        #region PRIVATE_METHODS

        private void ContinueListener()
        {
            if(this.udp.Client == null)
            {
                this.udp = new UdpClient(PORT);
            }
            this.udp.BeginReceive(Receive, new object());
        }

        private void Receive(IAsyncResult ar)
        {
            try
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, PORT);
                byte[] bytes = udp.EndReceive(ar, ref ip);
                ipClient = ip;
                string command = Encoding.ASCII.GetString(bytes);
                switch (command)
                {
                    case "START":
                        {
                            if (OnStart != null)
                                OnStart.Invoke(this, EventArgs.Empty);
                            break;
                        }
                }

                ContinueListener();
            }
            catch
            {
            }
        }

        #endregion

        #region PUBLIC_METHODS

        public void Open()
        {
            this.udp = new UdpClient(PORT);
            ContinueListener();
        }

        public void Send(string text)
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(text);
                udp.Send(bytes, bytes.Length, ipClient);
            }
            catch
            { 
            }
        }

        public void Close()
        {
            udp.Close();
        }

        #endregion
    }
}
