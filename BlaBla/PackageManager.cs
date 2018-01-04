using BlaBlaServer.Commands;
using Common;
using Common.Communication;
using System.Collections.Generic;
using System.Net.Sockets;

namespace BlaBlaServer
{
    public class PackageManager
    {
        internal Settings Settings;
        internal Communication Communication;
        internal CommandParser PackageReceivedParser;


        public PackageManager(Settings settings, Communication communication)
        {
            Settings = settings;
            Communication = communication;
            Communication.PackageReceived += CommandProcessor;
            PackageReceivedParser = new CommandParser(GetAvailableCommands());
        }

        private PackageManager() { }

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
