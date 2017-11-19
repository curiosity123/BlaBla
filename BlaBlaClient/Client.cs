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
        ICommunication<Command> Communication;
        public ClientCommandManager CommandManager;
        public ClientData Data = new ClientData();


        public Client(ISerialization serialization, string ip, int port)
        {
            Communication = new TcpClientCommunication(serialization, ip, port);
            CommandManager = new ClientCommandManager(Data, Communication);
            Communication.PackageReceived += CommandManager.EventProcessor;
        }
        private Client() { }

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
