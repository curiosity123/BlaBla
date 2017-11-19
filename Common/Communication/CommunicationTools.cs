using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Common.Communication
{
    public static class CommunicationTools
    {
        public static void Receive(ISerialization serializer, TcpClient Session, Action<TcpClient, Common.Command> MessageReceived)
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
                            Common.Command package = serializer.Deserialize<Command>(data.ToArray());
                            MessageReceived(Session, package);
                            data.Clear();
                        }
                    }

                    Thread.Sleep(1);
                }
            }).Start();
        }

        public static void Send<T>(ISerialization serializer, StreamWriter stream, T item)
        {
            byte[] data = serializer.Serialize<T>(item);
            stream.Write(Encoding.UTF8.GetString(data).ToCharArray());
            stream.Flush();
        }
    }
}
