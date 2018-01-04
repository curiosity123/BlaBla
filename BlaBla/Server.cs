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
        readonly ServerPackageManager PackageManager;
        readonly ServerSettings AppSettings;
        readonly ServerCommunication Communication;

        private Server(ISerialization serialization, string ip, int port)
        {
            AppSettings = new ServerSettings();
            Communication = new ServerCommunication(serialization, AppSettings, ip, port);
            PackageManager = new ServerPackageManager(AppSettings, Communication);
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
