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
        ISerialization serialization;
        ServerCommandManager CommandManager;
        ServerSettings Settings;
        TcpListenerCommunication Communication;


        private Server(ISerialization serialization, string ip, int port)
        {
            Settings = new ServerSettings();
            Communication = new TcpListenerCommunication(serialization, Settings, ip, port);
            CommandManager = new ServerCommandManager(Settings, Communication);

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
