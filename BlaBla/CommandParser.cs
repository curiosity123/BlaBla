using Common;
using Common.ICommandPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace BlaBlaServer
{
    public class CommandParser
    {
        readonly IEnumerable<ICommandFactory> availableCommands;

        public CommandParser(IEnumerable<ICommandFactory> availableCommands)
        {
            this.availableCommands = availableCommands;
        }


        internal ICommand ParseCommand(TcpClient Client,DataPackage param, ServerPackageManager manager)
        {
            var command = availableCommands.FirstOrDefault(Cmd => Cmd.Type == param.Type);
            if (command == null)
                return null;

             return command.MakeCommand(Client ,param, manager);
        }
    }
}
