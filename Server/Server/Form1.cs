using System;
using System.Windows.Forms;
using pyComm;

namespace Server
{
    public partial class Form1 : Form
    {
        pyHandler handler;

        public Form1()
        {
            InitializeComponent();

            handler = new pyHandler();
            handler.OnConnected += Handler_OnConnected;
            handler.OnStart += Handler_OnStart;
        }

        private void Handler_OnStart(object sender, EventArgs e)
        {
            this.Invoke(
                new Action(
                    () =>
                    {
                        textBox1.Text += "Command received: START" + Environment.NewLine;
                    }
                )); ;
        }

        private void Handler_OnConnected(object sender, EventArgs e)
        {
            this.Invoke(
                new Action(
                    () =>
                    {
                        textBox1.Text += "New connection from " + handler.PyAddress + Environment.NewLine;
                    }
                )); ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(button1.Text == "Start Listener")
            {
                textBox1.Text += "Listener Started." + Environment.NewLine;
                button1.Text = "Stop Listener";
                handler.Connect();
            }
            else
            {
                handler.Close();
                textBox1.Text += "Listener Stopped." + Environment.NewLine;
                button1.Text = "Start Listener";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            handler.Send(textBox2.Text);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            handler.Close();
        }
    }
}
