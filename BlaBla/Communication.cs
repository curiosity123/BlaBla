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
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Listening for new client...");
                    TcpClient client = Listener.AcceptTcpClient();
                    Settings.Sessions.Add(new Session() { Client = client, LastActivity = DateTime.UtcNow });
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Connected with new client " + client.Client.RemoteEndPoint.ToString());
                    CommunicationTools.CommunicationTools.Receive(Serialization, client, PackageReceived);
                    PrintTotalSessionsCount(Settings.Sessions.Count());
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Connection terminated... " + cs.Client.Client.RemoteEndPoint.ToString());
                    PrintTotalSessionsCount(Settings.Sessions.Count()-1);
                    cs.Client.Close();
                }

                Settings.Sessions.RemoveAll(x => x.Client.Connected == false);

                Thread.Sleep(1000);
            }
        }

        private void PrintTotalSessionsCount(int count)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Total active sessions: " +count.ToString());
        }

        public void Send<T>(TcpClient Client, T item)
        {
            if(CommunicationTools.CommunicationTools.IsConnected(Client))
                CommunicationTools.CommunicationTools.Send(Serialization, new StreamWriter(Client.GetStream()), item);
        }
    }

}
