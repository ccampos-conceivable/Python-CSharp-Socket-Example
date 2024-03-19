using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;


namespace pyComm
{
    public class pyHandler
    {
        #region CLASS_VARIABLES

        private TcpListener _listener;
        private Thread _listenerThread;
        private TcpClient client;
        private NetworkStream ns;

        private bool connected = false;

        #endregion

        #region PUBLIC_PROPERTIES

        public bool Connected
        {
            get { return connected; }
        }

        public string PyAddress { get; private set; }

        #endregion

        #region PUBLIC_EVENTS

        public event EventHandler<EventArgs> OnConnected;
        public event EventHandler<EventArgs> OnDisconnected;
        public event EventHandler<EventArgs> OnStart;

        #endregion

        #region PRIVATE_METHODS

        private void StartListener()
        {
            _listenerThread = new Thread(RunListener);
            _listenerThread.Start();
        }

        private void RunListener()
        {
            try
            {
                _listener = new TcpListener(IPAddress.Any, 60000);
                _listener.Start();

                TcpClient client = _listener.AcceptTcpClient();
                if (client.Connected)
                {
                    connected = true;
                    PyAddress = client.Client.RemoteEndPoint.ToString();
                    if(OnConnected != null)
                        OnConnected(this, EventArgs.Empty);
                    ThreadPool.QueueUserWorkItem(ProcessClient, client);
                }
            }
            catch
            {
                connected = false;
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
                        switch (command)
                        {
                            case "START":
                                {
                                    if (OnStart != null)
                                        OnStart.Invoke(this, EventArgs.Empty);
                                    break;
                                }
                        }
                    }
                }
            }
            catch
            {
                connected = false;
            }
        }

        #endregion

        #region PUBLIC_METHODS

        public void Connect()
        {
            StartListener();
        }

        public void Send(string text)
        {
            // Respond to Python
            if(connected)
            {
                ns.Write(ASCIIEncoding.ASCII.GetBytes(text), 0, ASCIIEncoding.ASCII.GetBytes(text).Length);
            }
        }

        public void Close()
        {
            connected = false;
            if (ns != null)
                ns.Close();
            if (client != null)
                client.Close();
            if (_listener != null)
                _listener.Server.Close();
        }

        #endregion
    }
}
