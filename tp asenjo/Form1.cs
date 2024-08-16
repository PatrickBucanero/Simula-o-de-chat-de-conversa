using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Reflection;

namespace tp_asenjo
{
    public partial class Form1 : Form
    {
        Thread mythread;
        Boolean flag = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void meuProcesso()
        {
            Socket socketreceber = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
            EndPoint endereco2 = new IPEndPoint((IPAddress.Any), 9060);
            byte[] data = new byte[1024];
            int qtdbytes;

            socketreceber.Bind(endereco2);
            while (flag)
            {
                qtdbytes = socketreceber.ReceiveFrom(data, ref endereco2);
                string msg = Encoding.ASCII.GetString(data, 0, qtdbytes);
                this.Invoke(new Action(() =>
                {
                    listBox1.Items.Add(msg);
                }));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Socket socketenviar = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
            IPEndPoint endereco1 = new IPEndPoint(IPAddress.Parse("172.16.212.26"), 9060);

            string msg = "Matheus: " + textBox1.Text;

            socketenviar.SendTo(Encoding.ASCII.GetBytes(msg), endereco1);

            listBox1.Items.Add(msg);

            textBox1.Clear();
            textBox1.Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mythread = new Thread(new ThreadStart(meuProcesso));
            flag = true;
            mythread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
    }
}
