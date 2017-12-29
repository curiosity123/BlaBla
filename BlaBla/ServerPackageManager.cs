using BlaBlaServer.Commands;
using Common;
using Common.Communication;
using Common.ICommandPattern;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace BlaBlaServer
{
    public class ServerPackageManager
    {
        internal ServerSettings Settings;
        internal ServerCommunication Communication;
        internal List<Conversation> Conversations;
        CommandParser PackageReceivedParser;

        public ServerPackageManager(ServerSettings settings, ServerCommunication communication, List<Conversation> conv)
        {
            Settings = settings;
            Communication = communication;
            Communication.PackageReceived += CommandProcessor;
            Conversations = conv;
            PackageReceivedParser = new CommandParser(GetAvailableCommands());
        }

        private ServerPackageManager() { }

        private static IEnumerable<ICommandFactory> GetAvailableCommands()
        {
            return new ICommandFactory[]
                {
                    new LoginCommand(),
                    new LogoutCommand(),
                    new RegisterCommand(),
                    new MessageCommand(),
                    new AliveCommand(),
                    new UsersCommand(),
                    new ConversationCommand()
                };
        }


        public void CommandProcessor(TcpClient Client, DataPackage Cmd)
                => PackageReceivedParser.ParseCommand(Client, Cmd, this)?.Execute();

    }
}
