using BlaBlaServer.Commands;
using Common;
using Common.Communication;
using System.Collections.Generic;
using System.Net.Sockets;

namespace BlaBlaServer
{
    public class ServerPackageManager
    {
        internal ServerSettings Settings;
        internal ServerCommunication Communication;
        internal CommandParser PackageReceivedParser;


        public ServerPackageManager(ServerSettings settings, ServerCommunication communication)
        {
            Settings = settings;
            Communication = communication;
            Communication.PackageReceived += CommandProcessor;
            PackageReceivedParser = new CommandParser(GetAvailableCommands());
        }

        private ServerPackageManager() { }

        private static IEnumerable<ICommandFactory> GetAvailableCommands()
            => new ICommandFactory[]
                {
                    new LoginCommand(),
                    new LogoutCommand(),
                    new RegisterCommand(),
                    new MessageCommand(),
                    new AliveCommand(),
                    new UsersCommand(),
                };

        public void CommandProcessor(TcpClient Client, DataPackage Cmd)
            => PackageReceivedParser.ParseCommand(Client, Cmd, this)?.Execute();

    }
}
