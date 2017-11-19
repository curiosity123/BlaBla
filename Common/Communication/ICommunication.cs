using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Common.Communication
{
    public interface ICommunication<T>
    {
        void Connect();

        void Disconnect();

        void Send<T>(T item);

        event Action<TcpClient,T> PackageReceived;

        void StartSendingAlivePackage(User user);

        void StopSendingAlivePackage();
    }
}
