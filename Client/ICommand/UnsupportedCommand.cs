using System;
using System.Collections.Generic;
using System.Text;
using BlaBlaClient;
using Common;

namespace Client.ICommand
{
    public class UnsupportedCommand : ICommand, IClientCommandFactory
    {
        public string CommandName => "";

        public string Description => "";

        public Command Cmd { get; set; }
        public ClientSettings settings { get; set; }
        public IClientCommunication communication { get; set; }
        public List<Conversation> conversationList { get; set; }
        public PackageTypeEnum Type
        {
            get { return PackageTypeEnum.Unsupported; }
        }

        public void Execute()
        {
        }

        ICommand IClientCommandFactory.MakeCommand(Command Cmd, ClientSettings Settings, IClientCommunication Communication, List<Conversation> ConversationList)
        {
            return new LoginReceivedCommand() { Cmd = Cmd, settings = Settings, conversationList = conversationList, communication = Communication };
        }

    }
}
