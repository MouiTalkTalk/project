using System;
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

        // rank 업데이트를 위한 것들
        TcpListener listener;
        Thread from_form1;


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

            this.listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 16000 + username.Length);
            this.from_form1 = new Thread(new ThreadStart(Rank_update));
            from_form1.Start();
            btnConnect_Click(this, e);
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
                Invoke(new MethodInvoker(delegate ()
                {
                    clientState.Text = "연결 없음...";
                }));
                return; // 연결을 끝낸다.
            }

            while (stream != null)
            {
                stream = client.GetStream();
                byte[] buffer = new byte[PACKETSIZE];
                Packet packet = new Packet();

                try
                {
                    stream.Read(buffer, 0, buffer.Length);
                }
                catch
                {
                    return;
                }
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
            if (txtUserName.Text == "")
                return;

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

        private void Rank_update()
        {
            try
            {
                listener.Start();
                TcpClient tcpClient = this.listener.AcceptTcpClient();
                StreamReader streamReader = new StreamReader(tcpClient.GetStream());
            

                while(tcpClient.Connected)
                {
                    string message = streamReader.ReadLine();
                    string[] info = new string[3];
                    info = message.Split(',');

                    UserInfo send = new UserInfo();
                    byte[] buffer = new byte[PACKETSIZE];
                    send.Type = (int)PacketType.사용자정보;
                    send.TotalAsset = Convert.ToInt32(info[1]);
                    send.raiseRate = (double)(Convert.ToInt32(info[1])) / 1000000 - (double)1;
                    send.dayDay = info[2];

                    buffer = Packet.Serialize(send);
                    try
                    {
                        stream.Write(buffer, 0, buffer.Length);
                    }
                    catch
                    {
                        return;
                    }
                    stream.Flush();
                }
            }
            catch { return; }
        }

/*        private void HandleClientConnection(object obj)
        {   // 파일 핸들링 클라이언트
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();

            byte[] fileNameLengthBytes = new byte[4];
            stream.Read(fileNameLengthBytes, 0, 4);
            int fileNameLength = BitConverter.ToInt32(fileNameLengthBytes, 0);

            byte[] fileNameBytes = new byte[fileNameLength];
            stream.Read(fileNameBytes, 0, fileNameLength);
            string fileName = System.Text.Encoding.ASCII.GetString(fileNameBytes);

            byte[] fileLengthBytes = new byte[4];
            stream.Read(fileLengthBytes, 0, 4);
            int fileLength = BitConverter.ToInt32(fileLengthBytes, 0);

            byte[] fileBytes = new byte[fileLength]; // fileBytes 변수 선언
            int bytesRead = 0;
            while (bytesRead < fileLength)
            {
                int bytesToRead = Math.Min(1024, fileLength - bytesRead);
                int bytesReceived = stream.Read(fileBytes, bytesRead, bytesToRead);
                bytesRead += bytesReceived;
            }

            string filePath = Path.Combine(@"C:\Received", fileName);
            File.WriteAllBytes(filePath, fileBytes);

            Console.WriteLine("파일이 성공적으로 수신되었습니다: " + filePath);

            client.Close();
        }*/

    }
}
