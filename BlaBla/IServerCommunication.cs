using Common;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace BlaBlaServer
{
    public interface IServerCommunication
    {
        event Action<TcpClient, Command> PackageReceived;
        void Connect();
        void Disconnect();
        void Send<T>(TcpClient Client, T item);
    }
}
