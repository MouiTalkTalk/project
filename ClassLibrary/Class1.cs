using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Schema;

namespace ClassLibrary
{
    public enum PacketType
    {
        메시지 = 0,
        사용자정보 = 1,
        로그인
    }

    public enum PacketSendError
    {
        정상 = 0,
        에러
    }

    [Serializable]
    public class Packet
    {
        public int Type;
        public string payload;

        public Packet()
        {
            this.Type = 0;
        }

        public static byte[] Serialize(Object o)
        {
            MemoryStream ms = new MemoryStream(1024 * 4);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, o);
            return ms.ToArray();
        }

        public static Object Deserialize(byte[] bt)
        {
            MemoryStream ms = new MemoryStream(1024 * 4);
            foreach (byte b in bt) 
            {
                ms.WriteByte(b);
            }
            ms.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            Object obj = bf.Deserialize(ms);
            ms.Close();
            return obj;
        }
    }

    [Serializable]
    public class UserMessage : Packet
    {
        public string UserName;
    }

    [Serializable]
    public class UserInfo : Packet
    {
        public int TotalAsset; // 자산
        public double raiseRate; // 수익률
        public string dayDay;
        public UserInfo()
        {
            this.TotalAsset = 0;
            this.raiseRate = 0;
            this.dayDay = string.Empty;
        }
    }

    [Serializable]
    public class Login : Packet
    {
        public string LoginID;
        public string LoginPassword;
        
        public Login()
        {
            this.LoginID = string.Empty;
            this.LoginPassword = string.Empty;
        }
    }

}
