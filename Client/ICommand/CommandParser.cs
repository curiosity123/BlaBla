using BlaBlaClient;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.ICommand
{
    public class CommandParser
    {
        readonly IEnumerable<ICommandFactory> availableCommands;

        public CommandParser(IEnumerable<ICommandFactory> availableCommands)
        {
            this.availableCommands = availableCommands;
        }


        internal ICommand ParseCommand(Command param,ClientCommandManager manager)
        {
            var command = availableCommands.FirstOrDefault(Cmd => Cmd.Type == param.Type);
            if (command == null)
                return null;

             return command.MakeCommand(param, manager);
        }
    }
}
