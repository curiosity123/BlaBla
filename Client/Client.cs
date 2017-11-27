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
        IClientCommunication Communication;
        public IClientCommandManager CommandManager;
        public ClientSettings Settings = new ClientSettings();
        public List<Conversation> Conversations = new List<Conversation>();

        private Client(ISerialization serialization, string ip, int port)
        {
            Communication = new ClientCommunication(serialization, ip, port);
            CommandManager = new ClientCommandManager(Settings, Communication,Conversations);
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
