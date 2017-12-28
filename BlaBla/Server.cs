using Common;
using Common.Communication;
using Common.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BlaBlaServer
{
    public class Server
    {
        ServerPackageManager CommandManager;
        ServerSettings Settings;
        IServerCommunication Communication;
        List<Conversation> Conversation = new List<Conversation>();


        private Server(ISerialization serialization, string ip, int port)
        {
            Settings = new ServerSettings();
            Communication = new ServerCommunication(serialization, Settings, ip, port);
            CommandManager = new ServerPackageManager(Settings, Communication,Conversation);
        }

        private Server() { }

        public static Server Create(ISerialization serialization, string ip, int port)
        {
            Server server = new Server(serialization, ip, port);
            return server;
        }



        public void Start()
        {
            Communication.Connect();  
        }

        public void Stop()
        {
            Communication.Disconnect();
        }

    }


    
}
