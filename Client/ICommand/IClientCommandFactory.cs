using BlaBlaClient;
using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.ICommand

{
    public interface IClientCommandFactory
    {
        string CommandName { get; }
        string Description { get; }
        Command Cmd { get; set; }
        PackageTypeEnum Type { get; }
        ClientSettings settings { get; set; }
        IClientCommunication communication { get; set; }
        List<Conversation> conversationList { get; set; }
        

        ICommand MakeCommand(Command Cmd, ClientSettings Settings, IClientCommunication Communication, List<Conversation> ConversationList);
    }
}
