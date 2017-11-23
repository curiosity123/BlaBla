using Common;
using System.Net.Sockets;

namespace BlaBlaServer
{
    public interface IServerCommandManager
    {
        void CommandProcessor(TcpClient Client, Command Cmd);
    }
}