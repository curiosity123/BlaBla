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
    [XmlIgnore]
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
[XmlInclude(typeof(object))]
public class Command
{
    public PackageType Type;
    public User Content;
}


[XmlInclude(typeof(Message))]
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
            StringBuilder sb = new StringBuilder();
            List<byte> data = new List<byte>();
            while (stream.CanRead)
            {
                if (stream.CanRead && stream.DataAvailable)
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
                        Command package = Tools.DeserializeObject<Command>(data.ToArray());
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
    public static void Send<T>(StreamWriter stream, T item)
    {
        XmlSerializer xs = new XmlSerializer(typeof(T));
        xs.Serialize(stream, item);
    }


    public static T DeserializeObject<T>(byte[] xml)
    {
        MemoryStream memoryStream = new MemoryStream(xml);
        XmlSerializer xs = new XmlSerializer(typeof(T));
        return (T)xs.Deserialize(memoryStream);
    }
}