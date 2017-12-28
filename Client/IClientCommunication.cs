using Common;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace BlaBlaClient
{

    public interface IClientCommunication
    {
        event Action<TcpClient, DataPackage> PackageReceived;
        void Connect();
        void Disconnect();
        void Send<T>(T item);
        void StartSendingAlivePackage(User user);
        void StopSendingAlivePackage();

    }
}
