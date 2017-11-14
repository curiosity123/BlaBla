using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml.Serialization;


class Common { }


[XmlInclude(typeof(User))]
[XmlInclude(typeof(Guid))]
public class User
{
    public Guid Id;
    public string NickName;
    public string Password;
}


public enum PackageType
{
    Register,
    Login,
    Logout,
    Users,
    Message
}

[XmlInclude(typeof(Command))]
[XmlInclude(typeof(User))]
[XmlInclude(typeof(Guid))]
[XmlInclude(typeof(Message))]
public class  Command
{
    public PackageType Type;
    public Object Content;
}




public class Message
{
    public string MyMessage;
    public List<User> UserList;
}


public static class NetTools
{
    public static void Receiver(TcpClient Session, Action<TcpClient, Command> MessageReceived)
    {
        NetworkStream stream = Session.GetStream();
        new Thread(() =>
        {

            List<byte> data = new List<byte>();
            while (stream.CanRead)
            {
                if ( stream.DataAvailable)
                {
                    
                    byte[] bytes = new byte[100];
                    int size = stream.Read(bytes, 0, 100);
                    data.AddRange(bytes);
                    data.RemoveAll(x => x == '\0');

                }
                else
                {
                    if (data.Count > 0)
                    {
                        string s = Encoding.UTF8.GetString(data.ToArray());
                        Command package = Tools.DeserializeObject(data.ToArray());
                        MessageReceived(Session, package);
                        data.Clear();
                    }
                }

                Thread.Sleep(10);
            }
        }).Start();
    }
}
public static class Tools
{
    public static void Send(StreamWriter stream, Command item)
    {
        XmlSerializer xs = new XmlSerializer(typeof(Command));
        xs.Serialize(stream, item);
    }


    public static Command DeserializeObject(byte[] xml)
    {

        string s =Encoding.UTF8.GetString(xml);
        MemoryStream memoryStream = new MemoryStream(xml);
        XmlSerializer xs = new XmlSerializer(typeof(Command));
        return (Command)xs.Deserialize(memoryStream);
    }
}