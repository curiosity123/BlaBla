using BlaBlaServer;
using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlaBla
{
    class Program
    {
        static Server server;
        static void Main(string[] args)
        {
            Console.WriteLine("server started");
            server = new Server("127.0.0.1", 8000);
            server.Start();
            Console.ReadKey();
        }
    }

}
