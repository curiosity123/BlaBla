using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace Common
{
    public static class NetworkTools
    {
        public static void Receiver(TcpClient Session, Action<TcpClient, Common.Command> MessageReceived)
        {
            NetworkStream stream = Session.GetStream();
            new Thread(() =>
            {

                List<byte> data = new List<byte>();
                while (stream.CanRead)
                {
                    if (stream.DataAvailable)
                    {

                        byte[] bytes = new byte[100];
                        int size = stream.Read(bytes, 0, 100);
                        data.AddRange(bytes);
                        
                    }
                    else
                    {
                        if (data.Count > 0)
                        {
                            data.RemoveAll(x => x == '\0');
                            string s = Encoding.UTF8.GetString(data.ToArray());
                            Common.Command package = DeserializeObject(data.ToArray());
                            MessageReceived(Session, package);
                            data.Clear();
                        }
                    }

                    Thread.Sleep(1);
                }
            }).Start();
        }


        public static void Send(StreamWriter stream, Command item)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Command));
            xs.Serialize(stream, item);
        }


        public static Common.Command DeserializeObject(byte[] xml)
        {
            try
            {
                string s = Encoding.UTF8.GetString(xml);
                MemoryStream memoryStream = new MemoryStream(xml);
                XmlSerializer xs = new XmlSerializer(typeof(Command));
                return (Command)xs.Deserialize(memoryStream);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Data corrupted!" + ex.ToString());
                return new Command();
            }
        }
    }
}
