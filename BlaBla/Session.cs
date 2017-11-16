using Common;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace BlaBlaServer
{
    public class Session
    {
        public DateTime LastActivity;
        public TcpClient Client;
        public User User;
    }
}

