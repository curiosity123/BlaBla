using Common.Communication;
using Common.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace Common
{


    public class TcpClientCommunication
    {
        private string Ip;
        private int Port;
        TcpClient tcpClient;
        StreamWriter clientStreamWriter;
        public event Action<TcpClient,Command> PackageReceived;

        ISerialization serializer;

        public TcpClientCommunication(ISerialization serializer, string ip, int port)
        {
            this.serializer = serializer;
            this.Ip = ip;
            this.Port = port;
        }
        private TcpClientCommunication() { }


        public void Connect()
        {
            tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect(Ip, Port);
                Receive(PackageReceived);
            }
            catch
            {
                Console.WriteLine("Cant connect");
            }

            clientStreamWriter = new StreamWriter(tcpClient.GetStream());
        }

        public bool IsAlive = false;
        public void Disconnect()
        {
            IsAlive = false;
            Thread.Sleep(2000);
            tcpClient.Close();
        }

        public void Receive(Action<TcpClient, Common.Command> MessageReceived)
        {
            CommunicationTools.Receive(serializer, tcpClient, MessageReceived);
        }

        public void Send<T>(T item)
        {
            CommunicationTools.Send(serializer, new StreamWriter(tcpClient.GetStream()), item);
        }

        private void Alive(User CurrentUser)
        {
            Command cmd = new Command() { Type = PackageTypeEnum.Alive, Content = CurrentUser };
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
