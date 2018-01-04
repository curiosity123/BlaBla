using BlaBlaClient;
using Client.Commands;
using Common;
using Common.ICommandPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class CommandParser
    {
        readonly IEnumerable<ICommandFactory> availableCommands;

        public CommandParser(IEnumerable<ICommandFactory> availableCommands)
        {
            this.availableCommands = availableCommands;
        }


        internal ICommand ParseCommand(DataPackage param, PackageManager manager)
        {
            var command = availableCommands.FirstOrDefault(Cmd => Cmd.Type == param.Type);
            if (command == null)
                return new UnsupportedCommand() { Cmd = param, Manager = manager };

            return command.MakeCommand(param, manager);
        }
    }
}
