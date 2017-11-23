using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using BlaBlaServer;
using Common.Serialization;
using System.Linq;
using System.IO;

namespace Common.Communication
{
    public class ServerCommunication: IServerCommunication
    {
        private string Ip;
        private int Port;
        private TcpListener Listener;
        private bool isRunning;
        private ISerialization serialization = new XmlSerialization();
        private ServerSettings Settings;

        public event Action<TcpClient, Command> PackageReceived;



        public ServerCommunication(ISerialization serialization, ServerSettings settings, string ip, int port)
        {
            this.serialization = serialization;
            this.Settings = settings;
            Ip = ip;
            Port = port;
        }
        private ServerCommunication(){ }


        public void Connect()
        {
            Listener = new TcpListener(IPAddress.Parse(Ip), Port);
            Listener.Start();
            new Thread(SessionWorker).Start();
        }

        public void Disconnect()
        {
            isRunning = false;
        }


        private void SessionWorker()
        {
            isRunning = true;
            new Thread(SessionRemoverWorker).Start();
            while (isRunning)
            {
                try
                {
                    Console.WriteLine("Listening for new cleint...");
                    TcpClient client = Listener.AcceptTcpClient();
                    Settings.Sessions.Add(new Session() { Client = client, LastActivity = DateTime.UtcNow });
                    Console.WriteLine("Connected with new client " + client.Client.RemoteEndPoint.ToString());
                    CommunicationTools.Receive(serialization, client, PackageReceived);
                }
                catch
                {
                    Console.WriteLine("Server stopped");
                    isRunning = false;
                    break;
                }
            }
        }
        private void SessionRemoverWorker()
        {
            while (isRunning)
            {
                var session = from x in Settings.Sessions where x.LastActivity.AddSeconds(10) < DateTime.UtcNow select x;

                foreach (Session cs in session)
                {
                    Console.WriteLine("Connection terminated... " + cs.Client.Client.RemoteEndPoint.ToString());
                    cs.Client.Close();
                }

                Settings.Sessions.RemoveAll(x => x.Client.Connected == false);

                Thread.Sleep(1000);
            }
        }

        public void Send<T>(TcpClient Client, T item)
        {
            if(CommunicationTools.IsConnected(Client))
                CommunicationTools.Send(serialization, new StreamWriter(Client.GetStream()), item);
        }
    }

}
