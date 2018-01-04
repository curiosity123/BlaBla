using BlaBlaServer.Commands;
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


        internal ICommand ParseCommand(TcpClient Client,DataPackage param, PackageManager manager)
        {
            var command = availableCommands.FirstOrDefault(Cmd => Cmd.Type == param.Type);
            if (command == null)
                return new UnsupportedCommand() { Client = Client, Cmd = param, Manager = manager };

             return command.MakeCommand(Client ,param, manager);
        }
    }
}
