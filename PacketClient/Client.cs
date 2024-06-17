﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary;
using System.Threading;
using System.IO;

namespace PacketClient
{
    public partial class Client : Form
    {
        TcpClient client = new TcpClient();
        NetworkStream stream = default(NetworkStream);
        Thread chat;

        string username = string.Empty;
        public const int PACKETSIZE = 1024 * 4;

        public Client()
        {
            InitializeComponent();
        }

        public Client(string name)
        {
            InitializeComponent();
            this.username = name;
        }

        private void Client_Load(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;
            txtUserName.Text = username;
            timer1.Interval = 1000;
        }

        private void GetMessage() // 서버로부터 메시지 받는 함수, 스레드로 실행된다.
        {
            // 먼저 서버와 userName을 주고 받아야함.
            StreamWriter writer = new StreamWriter(stream);
            StreamReader reader = new StreamReader(stream);

            writer.WriteLine(username);
            writer.Flush();
            string verify = reader.ReadLine();
            if (!verify.Equals("user name 승인됨")) // user name이 겹치는 경우 승인이 안 된다.
            {
                MessageBox.Show(verify);
                clientState.Text = "연결 없음...";
                return; // 연결을 끝낸다.
            }

            while (stream != null)
            {
                stream = client.GetStream();
                byte[] buffer = new byte[PACKETSIZE];
                Packet packet = new Packet();

                stream.Read(buffer, 0, buffer.Length);
                packet = (Packet)Packet.Deserialize(buffer);
                switch ((int)packet.Type)
                {
                    case (int)PacketType.메시지:
                        {
                            UserMessage userMessage = (UserMessage)packet;
                            string message = userMessage.payload;
                            DisplayText(message);
                            break;
                        }
                }
            }
        }

        private void DisplayText(string message)
        {
            Invoke(new MethodInvoker(delegate
            {
                richUserText.AppendText(message + "\r\n");
                richUserText.ScrollToCaret();
            }));
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtMessage.Text == string.Empty) // 아무것도 안 써져 있는 경우
            {
                return; // 아무것도 안 한다.
            }
            UserMessage send = new UserMessage();
            byte[] buffer = new byte[PACKETSIZE];

            send.Type = (int)PacketType.메시지;
            send.payload = txtMessage.Text;
            send.UserName = username;

            buffer = Packet.Serialize(send);
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            txtMessage.Text = "";
        }

        private void txtMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSend_Click(this, e);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text.Equals("서버와 연결"))
            {
                try
                {
                    btnConnect.Text = "연결 끊기";
                    btnConnect.BackColor = Color.Beige;
                    client.Connect(IPAddress.Parse("127.0.0.1"), 15000);
                    stream = client.GetStream();

                    username = txtUserName.Text;
                    chat = new Thread(new ThreadStart(GetMessage)); // 연결이 되면 메시지를 받아주는 스레드 생성
                    clientState.Text = "서버와 연결됨";
                    chat.IsBackground = true;
                    chat.Start();
                    timer1.Start();
                }
                catch
                {
                    MessageBox.Show("서버와 연결을 실패했습니다.");
                }
            }
            else
            {
                chat.Abort();

                UserMessage send = new UserMessage();
                byte[] buffer = new byte[PACKETSIZE];

                send.Type = (int)PacketType.메시지;
                send.payload = "LeaveChat";

                buffer = Packet.Serialize(send);
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
                btnConnect.Text = "서버와 연결";
                btnConnect.BackColor = Color.White;
            }
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            if (txtUserName.Text.Length > 0)
            {
                btnConnect.Enabled = true;
            }
            else
                btnConnect.Enabled = false;
        }

        private void Client_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (stream != null)
                stream.Close(); // 폼만 먼저 닫았을 경우에 이곳에서 오류가 난다.
            stream = null;
            client.Close();
            client = null;
        }

        int day_temp = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
/*            UserInfo send = new UserInfo();
            byte[] buffer = new byte[PACKETSIZE];

            send.Type = (int)PacketType.사용자정보;
            send.TotalAsset = 1000 + day_temp * 100;
            send.raiseRate = (double)day_temp;

            buffer = Packet.Serialize(send);
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            day_temp++;*/
        }

        public void TickByForm(string username, int totalAsset, string day)
        {
            UserInfo send = new UserInfo();
            byte[] buffer = new byte[PACKETSIZE];
            send.Type = (int)PacketType.사용자정보;
            send.TotalAsset = totalAsset;
            send.raiseRate = (double) (totalAsset / 1000000);
            send.dayDay = day;

            buffer = Packet.Serialize(send);
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

        }
    }
}
