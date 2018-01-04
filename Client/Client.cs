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
        ClientCommunication Communication;
        public PackageManager PackageManager;
        public ClientSettings Settings = new ClientSettings();


        private Client(ISerialization serialization, string ip, int port)
        {
            Communication = new ClientCommunication(serialization, ip, port);
            PackageManager = new PackageManager(Settings, Communication);
        }

        private Client() { }

        public static Client Create(ISerialization serialization, string ip, int port)
        {
            Client client = new Client(serialization, ip, port);
            return client;
        }


        public void Run()
        {
            Communication.Connect();
        }
        public void Stop()
        {
            Communication.Disconnect();
        }
    }
}
