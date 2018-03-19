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
        public bool IsDead => LastActivity.AddSeconds(10) < DateTime.UtcNow;
        public TcpClient Client;
        public User User;
    }
}

