using BlaBlaServer;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Common.Communication
{
    public class Communication
    {
        private string Ip;
        private int Port;
        private TcpListener Listener;
        private bool isRunning;
        private ISerialization Serialization;
        private Settings Settings;

        public event Action<TcpClient, DataPackage> PackageReceived;



        public Communication(ISerialization serialization, Settings settings, string ip, int port)
        {
            this.Serialization = serialization;
            this.Settings = settings;
            Ip = ip;
            Port = port;
        }
        private Communication(){ }


        public void Start()
        {
            Listener = new TcpListener(IPAddress.Parse(Ip), Port);
            Listener.Start();
            new Thread(SessionWorker).Start();
        }

        public void Stop()
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
                    Console.WriteLine("Listening for new client...");
                    TcpClient client = Listener.AcceptTcpClient();
                    Settings.Sessions.Add(new Session() { Client = client, LastActivity = DateTime.UtcNow });
                    Console.WriteLine("Connected with new client " + client.Client.RemoteEndPoint.ToString());
                    CommunicationTools.CommunicationTools.Receive(Serialization, client, PackageReceived);
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
            if(CommunicationTools.CommunicationTools.IsConnected(Client))
                CommunicationTools.CommunicationTools.Send(Serialization, new StreamWriter(Client.GetStream()), item);
        }
    }

}
