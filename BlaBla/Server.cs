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
        readonly PackageManager packageManager;
        readonly Settings settings;
        readonly Communication communication;

        private Server(ISerialization serialization, string ip, int port)
        {
            settings = new Settings();
            communication = new Communication(serialization, settings, ip, port);
            packageManager = new PackageManager(settings, communication);
        }

        private Server() { }

        public static Server Create(ISerialization serialization, string ip, int port)
        {
            Server server = new Server(serialization, ip, port);
            return server;
        }



        public void Start()
        {
            communication.Start();
        }

        public void Stop()
        {
            communication.Stop();
        }

    }


    
}
