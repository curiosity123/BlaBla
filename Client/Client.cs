using Client;
using Common;
using Common.Communication;
using Common.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BlaBlaClient
{

    public class Client
    {
        Communication communication;
        public PackageManager packageManager;
        public Settings settings = new Settings();


        private Client(ISerialization serialization, string ip, int port)
        {
            communication = new Communication(serialization, ip, port);
            packageManager = new PackageManager(settings, communication);
        }

        private Client() { }

        public static Client Create(ISerialization serialization, string ip, int port)
        {
            Client client = new Client(serialization, ip, port);
            return client;
        }


        public void Run()
        {
            communication.Connect();
        }
        public void Stop()
        {
            communication.Disconnect();
        }
    }
}
