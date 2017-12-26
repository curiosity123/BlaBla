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
        readonly IEnumerable<IClientCommandFactory> availableCommands;

        public CommandParser(IEnumerable<IClientCommandFactory> availableCommands)
        {
            this.availableCommands = availableCommands;
        }


        internal ICommand ParseCommand(Command param, ClientSettings Settings, IClientCommunication Communication, List<Conversation> ConversationList)
        {
            var command = availableCommands.FirstOrDefault(cmd => cmd.Type == param.Type);
            if (command == null)
                return null;
             return command.MakeCommand(param, Settings, Communication, ConversationList);
        }
    }
}
