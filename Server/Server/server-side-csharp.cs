using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System;


namespace Server
{
    public class Server
    {
        private TcpListener _listener;
        private Thread _listenerThread;

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
                            //textBox1.Text += string.Format("\nNew connection from {0}", client.Client.RemoteEndPoint) + Environment.NewLine;
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
            TcpClient client = state as TcpClient;
            NetworkStream ns = client.GetStream();
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
                                    //textBox1.Text += "Command received: " + command + Environment.NewLine;
                                }
                            )); ;
                        if (command.Contains("INIT"))
                        {
                            // Respond acknowledge to Python
                            ns.Write(ASCIIEncoding.ASCII.GetBytes("ACK"), 0, ASCIIEncoding.ASCII.GetBytes("ACK").Length);

                            // Do wherever unity application need to do...
                            this.Invoke(
                                new Action(
                                    () =>
                                    {
                                        //textBox1.Text += "Doing Unity stuff..." + Environment.NewLine;
                                    }
                                )); ;
                        }
                        ns.Close();
                        client.Close();
                    }
                }
                this.Invoke(
                    new Action(
                        () =>
                        {
                            //textBox1.Text += "Communication closed." + Environment.NewLine;
                        }
                    )); ;
            }
            catch (Exception ex)
            {
                // Log a communication error
            }
        }
    }
}