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
using System.Security.Cryptography.X509Certificates;
using System.CodeDom;
using System.Configuration;



namespace PacketServer
{
    public partial class Server : Form
    {
        private List<User> users = new List<User>();

        private TcpListener listener;

        private bool ServerOn = false;

        private Thread server;

        public const int PACKETSIZE = 1024 * 4;

        public class User // list를 3개나 관리하지 말고, 그냥 이 class에 변수로 넣고, 클래스 list 하나만 관리하면 될 것 같은데
        {
            public string userName;
            public int totalAsset;
            public double raiseRate;
            public string dayDay;

            public TcpClient tcpclient;
            public NetworkStream networkstream;
            public Thread thread;

            public bool userOn;

            public User()
            {
                this.userName = string.Empty;
                this.totalAsset = 0;
                this.raiseRate = 0;
                this.dayDay = string.Empty;
                tcpclient = null;
                networkstream = null;
                thread = null;
                userOn = false;
            }

            public void SetUser(string userName, int totalAsset, double raiseRate, string dayDay)
            {
                this.userName=userName;
                this.totalAsset = totalAsset;
                this.raiseRate = raiseRate;
                this.dayDay = dayDay;
            }
        }

        public Server()
        {
            InitializeComponent();
        }

        public void Server_Run()
        {
            this.listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 15000);
            this.listener.Start();
            this.ServerOn = true;

            while (this.ServerOn) // 서버가 종료되기 전까지는 계속 돈다.
            {
                try
                {
                    TcpClient client = this.listener.AcceptTcpClient();

                    if (client.Connected)
                    {
                        NetworkStream stream = client.GetStream();
                        StreamWriter writer = new StreamWriter(stream);
                        StreamReader reader = new StreamReader(stream);

                        string connected_user_name = reader.ReadLine(); // client는 연결 직후에 자신의 user name을 보내준다.

                        foreach (User i in this.users)
                        {
                            if (i.userName.Equals(connected_user_name)) // user list들 모두 돌면서, 일치하는 이름이 있으면 다시 설정하게 한다.
                            {   // streamWriter로 메시지를 보내준다. 그래서 클라이언트는 처음에 streamReader로 수신해야함.
                                writer.WriteLine("해당 user name은 이미 존재합니다. 이름을 바꿔주십시오.. 연결을 종료합니다...");
                                writer.Flush();
                                stream = null;
                            }
                        }
                        if (stream == null)
                        {
                            continue;
                        }
                            
                        writer.WriteLine("user name 승인됨"); // 이 메시지는 client 측에는 보이지 않아야한다.
                        writer.Flush();

                        User newUser = new User();
                        newUser.networkstream = stream;
                        newUser.tcpclient = client;
                        newUser.userName = connected_user_name;
                        newUser.totalAsset = 1000000; // 기본 자산 설정
                        Thread client_thread = new Thread(() => ClientHandler(newUser)); // 인자를 넣어주기 위해서 람다 표현식을 써줬다. 클라이언트에게 채팅을 받는 스레드를 개별로 만들어준다.
                        newUser.thread = client_thread;
                        this.users.Add(newUser); // 전부 설정해주고 list에 넣어준다.
                            
                        SetUserList(newUser);
                        client_thread.IsBackground = true; // 메인 스레드 종료시 같이 종료하게 한다.
                        client_thread.Start();

                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            this.ConnClientNum.Text = (int.Parse(this.ConnClientNum.Text) + 1).ToString(); // 접속한 클라이언트 수를 하나 늘려준다.
                        }));

                        DisplayText("System : [" + connected_user_name + "] 접속");
                        SendMessageAll(connected_user_name + "님이 입장하셨습니다.", "", true); // 접속한 모든 클라이언트에게 메시지를 보내준다.
                    }
                }
                catch
                {
                    break;
                }
            }
        }

        private void ClientHandler(User user)
        {
            TcpClient client;
            NetworkStream stream;
            byte[] buffer = new byte[PACKETSIZE];
            Packet packet = new Packet();
            string userName = user.userName;
            user.userOn = true;

            try
            {
                while (this.ServerOn && user.userOn)
                {
                    lock (this) // 동기화 블록 추가
                    {
                        if (user.tcpclient == null)
                        {
                            break; // 스트림이 없는 경우 종료
                        }

                        if (user.networkstream == null)
                        {
                            break; // 클라이언트가 없는 경우 종료
                        }
                    }

                    client = user.tcpclient;
                    stream = user.networkstream;

                    try
                    {
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0) // 연결이 종료된 경우
                        {
                            break;
                        }
                    }
                    catch (Exception e) // 오류 발생 시 예외 처리
                    {
                        MessageBox.Show(e.ToString());
                        break;
                    }

                    packet = (Packet)Packet.Deserialize(buffer);

                    switch ((int)packet.Type)
                    {
                        case (int)PacketType.메시지:
                            {
                                UserMessage packet_message = (UserMessage)packet;
                                string message = packet_message.payload;

                                if (message.Equals("LeaveChat"))
                                {
                                    string displayMessage = userName + "님이 종료하셨습니다.";

                                    user.userOn = false;
                                    DisplayText(displayMessage);
                                    SendMessageAll(displayMessage, userName, true);

                                    break;
                                }
                                else
                                {
                                    string displayMessage = userName + " : " + message;
                                    DisplayText(displayMessage);
                                    SendMessageAll(displayMessage, userName, true);
                                }
                                break;
                            }
                        case (int)PacketType.사용자정보:
                            {
                                UserInfo packet_userInfo = (UserInfo)packet;
                                foreach (User a in users)
                                {
                                    if (a.userName == userName)
                                    {
                                        a.totalAsset = packet_userInfo.TotalAsset;
                                        a.raiseRate = packet_userInfo.raiseRate;
                                        a.dayDay = packet_userInfo.dayDay;
                                    }
                                }
                                UpdateUserList();

                                break;
                            }
                        case (int)PacketType.로그인:
                            {
                                // 로그인 pakcet을 받으면 할 일
                                break;
                            }
                    }
                }
            }
            catch
            {
                DisplayText(userName + "의 연결에서 오류가 발생하여 연결을 종료하였습니다.");
                SendMessageAll("오류가 발생하여 연결을 종료합니다.", userName, false);
            }
            finally
            {
                lock (this) // 동기화 블록 추가
                {
                    foreach (User i in users)
                    {
                        if (i.userName.Equals(userName))
                        {
                            i.userOn = false;
                        }
                    }
                }
            }
        }


        public void SetUserList(User newUser) // 연결된 이후에 바로 호출됨.
        {
            this.Invoke(new MethodInvoker(delegate ()
            {
                UsrList.Items.Add(new ListViewItem(new string[] { newUser.userName, newUser.totalAsset.ToString(), newUser.raiseRate.ToString() + "%", "DAY 0" }));
            }));
        }

        public void UpdateUserList() // listView를 업데이트
        {
            this.Invoke((MethodInvoker)(delegate ()
            {
                UsrList.Items.Clear();
                foreach (User i in users)
                {
                    UsrList.Items.Add(new ListViewItem(new string[] { i.userName, i.totalAsset.ToString(), i.raiseRate.ToString() + "%", i.dayDay }));
                }
            }));
        }

        public void DisplayText(string text) // 받은 text를 그대로 richTextBox에 띄워주는 함수
        {
            lock (this) // 스레드를 여러개 쓰니까 동기화를 신경써주자
            {
                this.Invoke(new MethodInvoker(delegate ()
                {
                    richUsrText.AppendText(text + "\r\n");
                    richUsrText.ScrollToCaret();  // 스크롤을 가장 밑으로 내려준다.
                }));
            }
        }

        public void SendMessageAll(string message, string userName, bool flag) // 기본적으로 모두에게 보내는데, flag는 true이면 전체, false면 개별 송신이다. userName은 필요할 때 사용
        {
            lock (this) // 스레드를 여러개 쓰니까 동기화를 신경써주자
            {
                string date = DateTime.Now.ToString("yyyy.MM.dd. HH:mm:ss");
                byte[] buffer = new byte[PACKETSIZE];
                UserMessage user_message = new UserMessage();

                // 보낼 메시지를 설정하는 부분은 미리 해놓고 보내는 것만 foreach로 보낸다.
                if (flag) // 전체 송신의 경우는 보낼 메시지를 미리 설정한다.
                {
                    user_message.UserName = userName;
                    user_message.payload = date + "\r" + message;
                    Packet.Serialize(user_message).CopyTo(buffer, 0);
                }
                else // 개별 송신 -> 해당 userName에게만 송신한다.
                {
                    user_message.UserName = userName;
                    user_message.payload = message;
                    Packet.Serialize(user_message).CopyTo(buffer, 0);
                    
                    lock(this)
                    {
                        foreach(User i in users)
                        {
                            if (i.userName.Equals(userName) && i.userOn != false)
                            {
                                i.networkstream.Write(buffer, 0, buffer.Length);
                                i.networkstream.Flush();
                                return;
                            }
                        }
                    }
                }

                // 전체 송신
                lock(this)
                {
                    foreach (User i in this.users)
                    {
                        try
                        {
                            if (i.userOn == false)
                                continue;
                            i.networkstream.Write(buffer, 0, buffer.Length);
                            i.networkstream.Flush();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString()); // 오류 발생시 예외 처리
                        }
                    }
                }
            }
        }

        private void Server_Load(object sender, EventArgs e)
        {
            this.server = new Thread(new ThreadStart(Server_Run));
            this.server.Start();
            this.UsrList.View = View.Details;
            this.UsrList.Columns.Add("name", "이름");
            this.UsrList.Columns.Add("TotalAsset", "총자산");
            this.UsrList.Columns.Add("RaiseRate", "수익률");
            this.UsrList.Columns.Add("dayDay", "라운드");
            this.ServerState.Text = "Run";
        }

        private void Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.listener.Stop();

            foreach (User i in this.users)
            {
                if (i.tcpclient != null)
                    i.tcpclient.Close();
                if (i.networkstream != null)
                    i.networkstream.Close();
                if (i.thread != null)
                    i.thread.Abort();
            }
            users.Clear();
            this.server.Abort();
            Application.Exit();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string displayMessage = "Admin : " + txtMessage.Text;
            DisplayText(displayMessage);
            SendMessageAll(displayMessage, "Admin", true);
            txtMessage.Text = "";
        }

        private void txtMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSend_Click(this, e);
            }
        }

/*        private void SendFileToServer(string filePath)
        { // 파일 핸들링 서버
            try
            {
                string serverIP = "127.0.0.1";
                int serverPort = 9876;

                using (TcpClient client = new TcpClient(serverIP, serverPort))
                {
                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] fileNameBytes = System.Text.Encoding.ASCII.GetBytes(Path.GetFileName(filePath));
                        byte[] fileNameLengthBytes = BitConverter.GetBytes(fileNameBytes.Length);
                        stream.Write(fileNameLengthBytes, 0, 4);
                        stream.Write(fileNameBytes, 0, fileNameBytes.Length);

                        byte[] fileBytes = File.ReadAllBytes(filePath);
                        byte[] fileLengthBytes = BitConverter.GetBytes(fileBytes.Length);
                        stream.Write(fileLengthBytes, 0, 4);
                        stream.Write(fileBytes, 0, fileBytes.Length);
                    }
                }

                MessageBox.Show("파일이 성공적으로 전송되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("파일 전송 중 오류가 발생했습니다: " + ex.Message);
            }
        }*/



    } // form
} // namespace
