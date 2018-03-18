using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Common.CommunicationTools;

namespace Common
{



    public class Communication
    {
        private string Ip;
        private int Port;
        TcpClient tcpClient;
        StreamWriter clientStreamWriter;
        public event Action<DataPackage> PackageReceived;

        ISerialization serializer;
        private bool IsAlive = false;

        public Communication(ISerialization serializer, string ip, int port)
        {
            this.serializer = serializer;
            this.Ip = ip;
            this.Port = port;
        }
        private Communication() { }


        public void Connect()
        {
            tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect(Ip, Port);            
                CommunicationTools.CommunicationTools.Receive(serializer, tcpClient, (x, y) => PackageReceived?.Invoke(y));
            }
            catch
            {
                Console.WriteLine("Cant connect");
            }

            clientStreamWriter = new StreamWriter(tcpClient.GetStream());
        }

 

        public void Disconnect()
        {
            IsAlive = false;
            Thread.Sleep(2000);
            tcpClient.Close();
        }


        public void Send<T>(T item)
        {
            if (CommunicationTools.CommunicationTools.IsConnected(tcpClient))
                CommunicationTools.CommunicationTools.Send(serializer, new StreamWriter(tcpClient.GetStream()), item);
            else
            {
                Connect();
                CommunicationTools.CommunicationTools.Send(serializer, new StreamWriter(tcpClient.GetStream()), item);
            }
        }



        private void Alive(User CurrentUser)
        {
            DataPackage cmd = new DataPackage() { Type = PackageTypeEnum.Alive, Content = CurrentUser };
            Send(cmd);
        }

        public void StartSendingAlivePackage(User user)
        {
            if (IsAlive == false)
                new Thread(() =>
                {
                    IsAlive = true;
                    while (IsAlive)
                    {
                        Alive(user);
                        Thread.Sleep(1000);
                    }
                }).Start();
        }

        public void StopSendingAlivePackage()
        {
            IsAlive = false;
        }
    }




}
