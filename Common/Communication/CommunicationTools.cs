using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Common.CommunicationTools
{
    public static class CommunicationTools
    {
        public static void Receive(ISerialization serializer, TcpClient Session, Action<TcpClient, Common.DataPackage> MessageReceived)
        {
            NetworkStream stream = Session.GetStream();
            new Thread(() =>
            {

                StringBuilder data = new StringBuilder();
                while (stream.CanRead)
                {
                    if (stream.DataAvailable)
                    {

                        byte[] bytes = new byte[100];
                        int size = stream.Read(bytes, 0, 100);
                        data.Append(Encoding.UTF8.GetString(bytes));
                    }
                    else
                    {

                        if (data.Length > 0)
                        {
                            string[] packs = data.ToString().Split('\0');
                            packs = packs.Where(w => w != "").ToArray();
                            foreach (string s in packs)
                            {
                                Common.DataPackage package = serializer.Deserialize<DataPackage>(Encoding.ASCII.GetBytes(s));
                                MessageReceived(Session, package);
                            }
                            data.Clear();
                        }
                    }
                    Thread.Sleep(1);
                }
            }).Start();
        }

        public static void Send<T>(ISerialization serializer, StreamWriter stream, T item)
        {
            try
            {
                byte[] data = serializer.Serialize<T>(item);
                stream.Write(Encoding.UTF8.GetString(data).ToCharArray());
                stream.Write('\0');
                stream.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static bool IsConnected(TcpClient Client)
        {
            try
            {
                if (Client.Client.Poll(0, SelectMode.SelectRead))
                {
                    byte[] buff = new byte[1];
                    if (Client.Client.Receive(buff, SocketFlags.Peek) == 0)
                        return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
