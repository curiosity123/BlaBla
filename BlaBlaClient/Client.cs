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

    class Client
    {
        TcpClientCommunication Communication;
        public ClientCommandManager CommandManager;
        public ClientSettings Settings = new ClientSettings();

        private Client(ISerialization serialization, string ip, int port)
        {
            Communication = new TcpClientCommunication(serialization, ip, port);
            CommandManager = new ClientCommandManager(Settings, Communication);
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
